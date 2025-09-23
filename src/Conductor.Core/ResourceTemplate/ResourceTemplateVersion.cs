namespace Conductor.Core.ResourceTemplate;

public sealed record ResourceTemplateVersion
{
    public required ResourceTemplateId TemplateId { get; init; }
    public required string Version { get; init; }
    public required ResourceTemplateVersionSource Source { get; init; }
    public required string Notes { get; init; }
    public required ResourceTemplateVersionState State { get; init; }
    public required DateTime CreatedAt { get; init; }

    internal static ResourceTemplateVersion Create(ResourceTemplateId templateId, string version,
        ResourceTemplateVersionSource source, string notes, ResourceTemplateVersionState state, DateTime createdAt)
    {
        return new ResourceTemplateVersion
        {
            TemplateId = templateId,
            Version = version,
            Source = source,
            Notes = notes,
            State = state,
            CreatedAt = createdAt,
        };
    }
}