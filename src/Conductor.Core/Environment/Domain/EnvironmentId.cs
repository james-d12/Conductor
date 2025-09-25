namespace Conductor.Core.Environment.Domain;

public readonly record struct EnvironmentId(Guid Value)
{
    public EnvironmentId() : this(Guid.NewGuid())
    {
    }
}