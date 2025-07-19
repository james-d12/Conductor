namespace Conductor.Domain.Models.Resource;

public readonly record struct ResourceTemplateId(Guid Id)
{
    public ResourceTemplateId() : this(Guid.NewGuid())
    {
    }
}

/// <summary>
/// Represents an external requirement that an application needs (e.g. A Cosmos Db with a Container)
/// </summary>
public sealed record ResourceTemplate
{
    public required ResourceTemplateId Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required ResourceTemplateProvider Provider { get; init; }
    public required ResourceTemplateType Type { get; init; }
}