namespace Conductor.Core.Modules.Deployment.Domain;

public enum DeploymentStatus
{
    Pending,
    Deployed,
    Failed,
    RolledBack
}