namespace Conductor.Domain.Models.Deployment;

public enum DeploymentStatus
{
    Pending,
    Deployed,
    Failed,
    RolledBack
}