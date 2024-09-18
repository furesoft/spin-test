using System.Collections.Specialized;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using Fermyon.Spin.Sdk;
using PipelineNet.MiddlewareResolver;
using PipelineNet.Pipelines;
using Project.Core.Attributes;
using Project.Core.Attributes.Verbs;
using HttpMethod = Fermyon.Spin.Sdk.HttpMethod;

namespace Project.Core;

public static class Router
{
    private static readonly Dictionary<(HttpMethod, string), Func<HttpRequest, Dictionary<string, string>, HttpResponse>> Routes = new();

    public static readonly IPipeline<HttpContext> Middleware = new Pipeline<HttpContext>(new ActivatorMiddlewareResolver());

    static JsonSerializerOptions options = new JsonSerializerOptions
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public static void RegisterRoute(HttpMethod method, string urlPattern,
        Func<HttpRequest, Dictionary<string, string>, HttpResponse> handler)
    {
        Routes[(method, urlPattern)] = handler;
    }

    public static void RegisterController<T>()
    {
        var methods = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        var controllerInstance = ServiceContainer.Current.GetService(typeof(T));

        foreach (var method in methods)
        {
            var routeAttribute = method.GetCustomAttribute<HttpRouteAttribute>();

            if (routeAttribute != null)
            {
                RegisterRoute(routeAttribute.Method, routeAttribute.Route, (req, routeParams) =>
                {
                    var context = new HttpContext(req, new HttpResponse(), method, new Uri("http://localhost/" + req.Url));
                    context.Query = HttpUtility.ParseQueryString(context.Url.Query.Split('?').LastOrDefault() ?? "");
                    context.Controller = controllerInstance;

                    Middleware.Execute(context);

                    if (context.Response.StatusCode == 0)
                    {
                        var parameters = GetMethodParameters(method, context, routeParams);
                        var response = method.Invoke(controllerInstance, parameters.ToArray());

                        context.Response = SetBody(context.Response, response);
                    }

                    return context.Response;
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
        else
        {
            response.BodyAsString = JsonSerializer.Serialize(result, options);
            response.Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            };
        }

        return response;
    }

    private static IEnumerable<object> GetMethodParameters(MethodInfo method, HttpContext context, Dictionary<string, string> routeParams)
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

            if (AddPathParameter(param, args, routeParams))
            {
                continue;
            }

            if (AddQueryParameter(param, args, context.Query))
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
                args.Add(JsonSerializer.Serialize(context.Request.Body.AsString(), parameterInfo.ParameterType, options));
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

    private static bool AddQueryParameter(ParameterInfo parameterInfo, List<object> args, NameValueCollection queryParams)
    {
        var attr = parameterInfo.GetCustomAttribute<PathQueryAttribute>();
        if (attr is not null)
        {
            var name = attr.Name ?? parameterInfo.Name;

            if (queryParams[name] != null)
            {
                args.Add(Convert.ChangeType(queryParams[name], parameterInfo.ParameterType));
                return true;
            }
        }

        return false;
    }

    private static bool AddPathParameter(ParameterInfo parameterInfo, List<object> args, Dictionary<string, string> routeParams)
    {
        var attr = parameterInfo.GetCustomAttribute<PathParameterAttribute>();
        if (attr is not null)
        {
            var name = attr.Name ?? parameterInfo.Name;

            if (routeParams.TryGetValue(name, out var param))
            {
                args.Add(Convert.ChangeType(param, parameterInfo.ParameterType)!);
                return true;
            }
        }

        return false;
    }

    private static bool TryMatchUrlPattern(string urlPattern, string requestUrl, out Dictionary<string, string> routeParams)
    {
        routeParams = new Dictionary<string, string>();

        if (urlPattern == requestUrl)
        {
            return true;
        }

        var patternParts = urlPattern.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var urlParts = requestUrl.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (patternParts.Length != urlParts.Length)
        {
            return false;
        }

        for (int i = 0; i < patternParts.Length; i++)
        {
            if (patternParts[i] == urlParts[i])
            {
                continue;
            }

            if (patternParts[i].StartsWith('{') && patternParts[i].EndsWith('}'))
            {
                var paramName = patternParts[i][1..^1];
                routeParams[paramName] = urlParts[i];
            }
            else if (patternParts[i] != urlParts[i])
            {
                return false;
            }
        }

        return true;
    }

    public static HttpResponse Route(HttpRequest request)
    {
        foreach (var route in Routes)
        {
            var ((method, urlPattern), handler) = route;

            if (method == request.Method && TryMatchUrlPattern(urlPattern, request.Url, out var routeParams))
            {
                return handler(request, routeParams);
            }
        }

        return new HttpResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            BodyAsString = "Route not found"
        };
    }
}