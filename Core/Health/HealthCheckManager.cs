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

    public AggregatedHealthCheckResult CheckAll()
    {
        var subsystemResults = _healthChecks
            .Select(check => new SubsystemHealthCheckResult(check.Key, check.Value.CheckHealth()))
            .ToArray();

        var isHealthy = subsystemResults.All(s => s.IsHealthy);
        return new AggregatedHealthCheckResult(isHealthy, subsystemResults);
    }
}