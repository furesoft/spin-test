using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using DotnetTest.Core.Attributes;
using DotnetTest.Core.Attributes.Verbs;
using Fermyon.Spin.Sdk;
using Newtonsoft.Json;
using PipelineNet.MiddlewareResolver;
using PipelineNet.Pipelines;
using Tavis.UriTemplates;
using HttpMethod = Fermyon.Spin.Sdk.HttpMethod;

namespace DotnetTest.Core;

public static class Router
{
    private static readonly Dictionary<(HttpMethod, UriTemplate), Func<HttpRequest, IDictionary<string, object>, HttpResponse>> Routes = new();

    public static readonly IPipeline<HttpContext> Middleware = new Pipeline<HttpContext>(new ActivatorMiddlewareResolver());

    public static void RegisterRoute(HttpMethod method, string urlPattern,
        Func<HttpRequest, IDictionary<string, object>, HttpResponse> handler)
    {
        var template = new UriTemplate(urlPattern);
        Routes[(method, template)] = handler;
    }

    public static void RegisterController<T>()
    {
        var methods = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        var controllerInstance = ServiceContainer.Current.GetService(typeof(T));
        var controllerRouteAttribute = typeof(T).GetCustomAttribute<HttpRouteAttribute>();

        foreach (var method in methods)
        {
            var routeAttribute = method.GetCustomAttribute<HttpRouteAttribute>();

            if (routeAttribute != null)
            {
                RegisterRoute(routeAttribute.Method, controllerRouteAttribute?.Route + routeAttribute.Route, (req, routeParams) =>
                {
                    try
                    {
                        var context = new HttpContext(req, new HttpResponse(), method,
                            new Uri("http://localhost/" + req.Url))
                        {
                            Controller = controllerInstance
                        };

                        Middleware.Execute(context);

                        if (context.Response.StatusCode == 0)
                        {
                            var parameters = GetMethodParameters(method, context, routeParams);
                            var response = method.Invoke(controllerInstance, parameters.ToArray());

                            context.Response = SetBody(context.Response, response);
                        }

                        return context.Response;
                    }
                    catch (Exception ex)
                    {
                        return new HttpResponse()
                        {
                            StatusCode = HttpStatusCode.BadGateway,
                            BodyAsString = ex.ToString()
                        };
                    }
                });
            }
        }
    }

    private static HttpResponse SetBody(HttpResponse response, object result)
    {
        response.StatusCode = HttpStatusCode.OK;

        if (result is string strResult)
        {
            response.BodyAsString = strResult;
            response.Headers = new Dictionary<string, string>
            {
                { "Content-Type", "text/plain" }
            };
        }
        else if (result is byte[] bytes)
        {
            response.BodyAsBytes = bytes;
            response.Headers = new Dictionary<string, string>()
            {
                { "Content-Type", "application/octet-stream" }
            };
        }
        else
        {
            response.BodyAsString = JsonConvert.SerializeObject(result);
            response.Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            };
        }

        return response;
    }

    private static IEnumerable<object> GetMethodParameters(MethodInfo method, HttpContext context, IDictionary<string, object> routeParams)
    {
        var methodParams = method.GetParameters();
        var args = new List<object>();

        foreach (var param in methodParams)
        {
            if (param.ParameterType == typeof(HttpContext))
            {
                args.Add(context);
                continue;
            }

            if (AddParameter(param, args, routeParams))
            {
                continue;
            }

            if (AddHeader(param, args, context.Request.Headers))
            {
                continue;
            }

            if (InjectRequest(context, param, args))
            {
                continue;
            }

            if (InjectBody(context, param, args))
            {
                continue;
            }

            args.Add(param.HasDefaultValue ? param.DefaultValue : null);
        }

        return args;
    }

    private static bool InjectBody(HttpContext context, ParameterInfo parameterInfo, List<object> args)
    {
        var attr = parameterInfo.GetCustomAttribute<RequestBodyAttribute>();
        if (attr is not null)
        {
            if (parameterInfo.ParameterType == typeof(byte[]))
            {
                args.Add(context.Request.Body.AsBytes().ToArray());
            }
            if (parameterInfo.ParameterType == typeof(string))
            {
                args.Add(context.Request.Body.AsString());
            }
            else
            {
                args.Add(JsonConvert.DeserializeObject(context.Request.Body.AsString(), parameterInfo.ParameterType));
            }

            return true;
        }

        return false;
    }

    private static bool InjectRequest(HttpContext context, ParameterInfo param, List<object> args)
    {
        if (param.ParameterType == typeof(HttpRequest))
        {
            args.Add(context.Request);
            return true;
        }

        return false;
    }

    private static bool AddHeader(ParameterInfo parameterInfo, List<object> args, IReadOnlyDictionary<string,string> requestHeaders)
    {
        var attr = parameterInfo.GetCustomAttribute<HeaderAttribute>();
        if (attr is not null)
        {
            var name = attr.Name ?? parameterInfo.Name;

            if (requestHeaders[name] != null)
            {
                args.Add(Convert.ChangeType(requestHeaders[name], parameterInfo.ParameterType));
                return true;
            }
        }

        return false;
    }

    private static bool AddParameter(ParameterInfo parameterInfo, List<object> args,
        IDictionary<string, object> routeParams)
    {
        var name = parameterInfo.Name!;

        if (routeParams.TryGetValue(name, out var param))
        {
            args.Add(Convert.ChangeType(param, parameterInfo.ParameterType)!);
            return true;
        }

        return false;
    }

    public static HttpResponse Route(HttpRequest request)
    {
        // Sort routes by specificity (longer templates first) to match more specific routes before generic ones
        var sortedRoutes = Routes
            .OrderByDescending(r => r.Key.Item2.ToString().Length)
            .ThenByDescending(r => r.Key.Item2.ToString().Count(c => c == '{'))
            .ToList();

        foreach (var route in sortedRoutes)
        {
            var ((method, template), handler) = route;

            if (method != request.Method) continue;

            var pattern = UriTemplate.CreateMatchingRegex(template.ToString());
            if (Regex.IsMatch(request.Url, pattern))
            {
                var parameters = template.GetParameters(new(request.Url), QueryStringParameterOrder.Any);
                return handler(request, parameters);
            }
        }

        return new HttpResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            BodyAsString = "Route not found"
        };
    }
}