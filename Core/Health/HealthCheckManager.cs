using DotnetTest.Core.Health.Checks;

namespace DotnetTest.Core.Health;

public class HealthCheckManager
{
    private readonly Dictionary<string, IHealthCheck> _healthChecks = new();

    public HealthCheckManager()
    {
        RegisterHealthCheck("General", new GeneralCheck());
    }

    public void RegisterHealthCheck(string name, IHealthCheck healthCheck)
    {
        _healthChecks.Add(name, healthCheck);
    }

    public HealthCheckResult CheckAll()
    {
        var results = _healthChecks.Select(check => check.Value.CheckHealth()).ToList();

        return results.All(h => h.IsHealthy) ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
    }
}