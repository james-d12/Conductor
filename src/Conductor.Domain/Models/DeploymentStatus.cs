namespace Conductor.Domain.Models;

public enum DeploymentStatus
{
    Pending,
    Deployed,
    Failed,
    RolledBack
}