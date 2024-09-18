namespace DotnetTest.Core.Health;

public class HealthCheckResult(bool isHealthy, string message)
{
    public static HealthCheckResult Healthy(string message = "Healthy")
    {
        return new HealthCheckResult(true, message);
    }

    public static HealthCheckResult Unhealthy(string message = "Unhealthy")
    {
        return new HealthCheckResult(false, message);
    }

    public bool IsHealthy { get; init; } = isHealthy;
    public string Message { get; init; } = message;

    public void Deconstruct(out bool IsHealthy, out string Message)
    {
        IsHealthy = this.IsHealthy;
        Message = this.Message;
    }
}
