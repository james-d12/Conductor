namespace Conductor.Core.Application.Domain;

public readonly record struct ApplicationId(Guid Value)
{
    public ApplicationId() : this(Guid.NewGuid())
    {
    }
}