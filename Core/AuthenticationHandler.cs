using System.Net;
using System.Reflection;
using System.Security.Principal;
using DotnetTest.Core.Attributes;
using Fermyon.Spin.Sdk;
using PipelineNet.Middleware;

namespace DotnetTest.Core;

public class AuthenticationHandler : IMiddleware<HttpContext>
{
    private AuthorizationAttribute GetAttribute(HttpContext context)
    {
        var controllerType = context.Controller.GetType();
        var attr = controllerType.GetCustomAttribute<AuthorizationAttribute>();

        if (attr != null)
        {
            return attr;
        }

        return context.Method.GetCustomAttribute<AuthorizationAttribute>();
    }

    public void Run(HttpContext context, Action<HttpContext> next)
    {
        var authAttribute = GetAttribute(context);

        if (authAttribute != null)
        {
            var token = context.Query["Authorization"] ?? string.Empty;
            var validator = ServiceContainer.Current.Resolve<ITokenValidator>();

            if (validator.Validate(token))
            {
                context.Identity = new GenericPrincipal(new GenericIdentity("admin"), ["admin"]);
            }
            else
            {
                context.Response = new HttpResponse
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    BodyAsString = "You are not authorized to access this resource."
                };

                return;
            }
        }

        next(context);
    }
}