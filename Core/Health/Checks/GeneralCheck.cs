namespace DotnetTest.Core.Health.Checks;

public class GeneralCheck : IHealthCheck
{
    public HealthCheckResult CheckHealth()
    {
        return HealthCheckResult.Healthy();
    }
}