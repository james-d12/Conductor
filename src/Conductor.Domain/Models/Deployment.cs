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
    public required CommitId CommitId { get; init; }
    public required DeploymentStatus Status { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }

    private Deployment()
    {
    }

    public static Deployment Create(ApplicationId applicationId, EnvironmentId environmentId, CommitId commitId)
    {
        return new Deployment
        {
            Id = new DeploymentId(),
            ApplicationId = applicationId,
            EnvironmentId = environmentId,
            CommitId = commitId,
            Status = DeploymentStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
    }

    public void MarkAsFailed()
    {
        Status = DeploymentStatus.Failed;
        UpdatedAt = DateTime.UtcNow;
    }
}