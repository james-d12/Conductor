namespace Conductor.Core.Resource;

public readonly record struct ResourceId(Guid Value)
{
    public ResourceId() : this(Guid.NewGuid())
    {
    }
}