namespace Conductor.Domain.Deployment;

public enum DeploymentStatus
{
    Pending,
    Deployed,
    Failed,
    RolledBack
}