namespace Conductor.Domain.Models.ResourceTemplate.Requests;

public sealed record CreateResourceTemplateVersionRequest
{
    public required ResourceTemplateId TemplateId { get; init; }
    public required string Version { get; init; }
    public required Uri Source { get; init; }
    public required string Notes { get; init; }
    public required DateTime CreatedAt { get; init; }
}