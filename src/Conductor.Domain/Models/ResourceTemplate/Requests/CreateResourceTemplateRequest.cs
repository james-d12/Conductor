namespace Conductor.Domain.Models.ResourceTemplate.Requests;

public sealed record CreateResourceTemplateRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required ResourceTemplateProvider Provider { get; init; }
    public required ResourceTemplateType Type { get; init; }
};