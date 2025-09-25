namespace Conductor.Core.Deployment.Domain;

public readonly record struct DeploymentId(Guid Value)
{
    public DeploymentId() : this(Guid.NewGuid())
    {
    }
}