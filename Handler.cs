using System.Net;
using Fermyon.Spin.Sdk;
using Project.Controller;
using Project.Core;

namespace Project;

public static class Handler
{
    [HttpHandler]
    public static HttpResponse HandleHttpRequest(HttpRequest request)
    {
        if (request.Url == Warmup.DefaultWarmupUrl)
        {
            return new HttpResponse
            {
                StatusCode = HttpStatusCode.OK,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "text/plain" },
                },
                BodyAsString = "warmup",
            };
        }
        try
        {
            return Router.Route(request);
        }
        catch (Exception ex)
        {
            return new HttpResponse { StatusCode = HttpStatusCode.Conflict, BodyAsString = ex.ToString()};
        }
    }

    static Handler()
    {
        Router.Middleware.Add<AuthenticationHandler>();

        Router.RegisterController<TestController>(); 
        Router.RegisterController<Project.Core.Health.HealthController>();
    }
}