namespace Conductor.Core.Deployment.Domain;

public enum DeploymentStatus
{
    Pending,
    Deployed,
    Failed,
    RolledBack
}