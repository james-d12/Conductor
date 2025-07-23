using Conductor.Domain.Models.ResourceTemplate;

namespace Conductor.Domain.Models.Resource;

public readonly record struct ResourceId(Guid Id)
{
    public ResourceId() : this(Guid.NewGuid())
    {
    }
}

public abstract record Resource
{
    public required ResourceId Id { get; set; }
    public required string Name { get; set; }
    public required ResourceTemplateVersion TemplateVersion { get; set; }
}