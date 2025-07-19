namespace Conductor.Domain.Models;

public readonly record struct DeploymentId(Guid Id)
{
    public DeploymentId() : this(Guid.NewGuid())
    {
    }
}

/// <summary>
/// Represents a deployed application to a specific environment.
/// </summary>
public sealed record Deployment
{
    public required DeploymentId Id { get; init; }
    public required ApplicationId ApplicationId { get; init; }
    public required EnvironmentId EnvironmentId { get; init; }
    public required DeploymentStatus Status { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public DateTime UpdatedAt { get; init; } = DateTime.Now;

    private Deployment()
    {
    }

    public static Deployment Create(ApplicationId applicationId, EnvironmentId environmentId)
    {
        return new Deployment
        {
            Id = new DeploymentId(),
            ApplicationId = applicationId,
            EnvironmentId = environmentId,
            Status = DeploymentStatus.Pending
        };
    }
}