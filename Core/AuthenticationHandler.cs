using System.Net;
using System.Reflection;
using Fermyon.Spin.Sdk;
using PipelineNet.Middleware;
using Project.Core.Attributes;

namespace Project.Core;

public class AuthenticationHandler : IMiddleware<HttpContext>
{
    public void Run(HttpContext context, Action<HttpContext> next)
    {
        var authAttribute = context.Method.GetCustomAttribute<AuthorizationAttribute>();

        if (authAttribute != null)
        {
            var userRole = context.Request.Headers.ContainsKey("Role") ? context.Request.Headers["Role"] : string.Empty;

            if (!authAttribute.Roles.Contains(userRole))
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