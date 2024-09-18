using System.Net;
using DotnetTest.Core.Attributes.Verbs;

namespace DotnetTest.Core.Health;

[HttpGet("/health")]
public class HealthController(HealthCheckManager healthCheckManager)
{
    [HttpGet("/")]
    public object GetIndex()
    {
        return healthCheckManager.CheckAll();
    }
}
