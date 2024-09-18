namespace Project.Core.Health;

using Project.Core.Health.Checks;

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

    public Dictionary<string, HealthCheckResult> CheckAll()
    {
        Dictionary<string, HealthCheckResult> results = new();

        foreach (var check in _healthChecks)
        {
            results.Add(check.Key, check.Value.CheckHealth());
        }

        return results;
    }
}