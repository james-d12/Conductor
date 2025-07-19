namespace Conductor.Domain.Models.Resource;

public sealed record ResourceTemplateVersion
{
    public required ResourceTemplateId TemplateId { get; init; }
    public required string Version { get; init; }
    public required Uri Source { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string Notes { get; init; }
}