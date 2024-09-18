using Project.Core.Attributes;
using Project.Core.Attributes.Verbs;

namespace Project.Core.Health;

[HttpGet("/health")]
public class HealthController
{
    private readonly HealthCheckManager _healthCheckManager;

    public HealthController(HealthCheckManager healthCheckManager)
    {
        _healthCheckManager = healthCheckManager;
    }

    [HttpGet("/")]
    public object GetIndex()
    {
        return _healthCheckManager.CheckAll();
    }
}
