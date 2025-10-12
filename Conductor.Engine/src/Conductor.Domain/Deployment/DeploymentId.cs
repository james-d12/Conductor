namespace Conductor.Domain.Deployment;

public readonly record struct DeploymentId(Guid Value)
{
    public DeploymentId() : this(Guid.NewGuid())
    {
    }
}