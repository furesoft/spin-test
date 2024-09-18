namespace DotnetTest.Core.Health;

public interface IHealthCheck
{
    HealthCheckResult CheckHealth();
}