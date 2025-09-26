namespace Conductor.Domain.Deployment.Domain;

public enum DeploymentStatus
{
    Pending,
    Deployed,
    Failed,
    RolledBack
}