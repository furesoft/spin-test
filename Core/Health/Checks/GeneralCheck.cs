namespace Project.Core.Health.Checks;

using Project.Core.Health;

public class GeneralCheck : IHealthCheck
{
    public HealthCheckResult CheckHealth()
    {
        return HealthCheckResult.Healthy();
    }
}