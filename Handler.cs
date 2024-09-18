using System.Net;
using DotnetTest.Controller;
using DotnetTest.Core;
using DotnetTest.Core.Health;
using Fermyon.Spin.Sdk;

namespace DotnetTest;

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
        ServiceContainer.Current.Register<HealthCheckManager> ();

        Router.Middleware.Add<AuthenticationHandler>();

        Router.RegisterController<TestController>();
        Router.RegisterController<HealthController>();
    }
}