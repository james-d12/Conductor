namespace Conductor.Domain.Models;

/// <summary>
/// Represents a deployed application to a specific environment.
/// </summary>
public record Deployment
{
    public required Guid Id { get; init; }
    public required ApplicationId ApplicationId { get; init; }
    public required EnvironmentId EnvironmentId { get; init; }

    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public DateTime UpdatedAt { get; init; } = DateTime.Now;
}