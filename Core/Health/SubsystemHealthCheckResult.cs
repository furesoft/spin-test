namespace DotnetTest.Core.Health;

public class SubsystemHealthCheckResult(string name, HealthCheckResult result)
{
    public string Name { get; init; } = name;
    public bool IsHealthy { get; init; } = result.IsHealthy;
    public string Message { get; init; } = result.Message;
}