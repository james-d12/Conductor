using Conductor.Domain.Deployment.Requests;
using Conductor.Domain.Environment.Domain;
using ApplicationId = Conductor.Domain.Application.Domain.ApplicationId;

namespace Conductor.Domain.Deployment.Domain;

/// <summary>
/// Represents a deployed application to a specific environment.
/// </summary>
public sealed record Deployment
{
    public required DeploymentId Id { get; init; }
    public required ApplicationId ApplicationId { get; init; }
    public required EnvironmentId EnvironmentId { get; init; }
    public required CommitId CommitId { get; init; }
    public required DeploymentStatus Status { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }

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

    public static Deployment Create(CreateDeploymentRequest request)
    {
        return Create(request.ApplicationId, request.EnvironmentId, request.CommitId);
    }
}