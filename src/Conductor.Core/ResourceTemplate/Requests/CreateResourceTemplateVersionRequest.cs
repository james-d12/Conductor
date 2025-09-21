using Conductor.Core.ResourceTemplate.Domain;

namespace Conductor.Core.ResourceTemplate.Requests;

public sealed record CreateResourceTemplateVersionRequest
{
    public required ResourceTemplateId TemplateId { get; init; }
    public required string Version { get; init; }
    public required ResourceTemplateVersionSource Source { get; init; }
    public required string Notes { get; init; }
    public required ResourceTemplateVersionState State { get; init; }
    public required DateTime CreatedAt { get; init; }
}