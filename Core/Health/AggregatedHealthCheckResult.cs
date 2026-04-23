namespace DotnetTest.Core.Health;

public class AggregatedHealthCheckResult
{
    public bool IsHealthy { get; init; }
    public string Status { get; init; }
    public SubsystemHealthCheckResult[] Subsystems { get; init; }

    public AggregatedHealthCheckResult(bool isHealthy, SubsystemHealthCheckResult[] subsystems)
    {
        IsHealthy = isHealthy;
        Status = isHealthy ? "Healthy" : "Unhealthy";
        Subsystems = subsystems;
    }
}