namespace Project.Core.Health;

public interface IHealthCheck
{
    HealthCheckResult CheckHealth();
}