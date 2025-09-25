namespace Conductor.Core.ResourceTemplate.Domain;

public readonly record struct ResourceTemplateId(Guid Value)
{
    public ResourceTemplateId() : this(Guid.NewGuid())
    {
    }
}