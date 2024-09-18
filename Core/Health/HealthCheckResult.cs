namespace Project.Core.Health;

public record HealthCheckResult(bool IsHealthy, string Message)
{
    public static HealthCheckResult Healthy(string message = "Healthy")
    {
        return new HealthCheckResult(true, message);
    }

    public static HealthCheckResult Unhealthy(string message = "Unhealthy")
    {
        return new HealthCheckResult(false, message);
    }
}
