using Conductor.Domain.ResourceTemplate.Domain;

namespace Conductor.Domain.ResourceTemplate.Requests;

public sealed record CreateResourceTemplateRequest
{
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string Description { get; init; }
    public required ResourceTemplateProvider Provider { get; init; }
};