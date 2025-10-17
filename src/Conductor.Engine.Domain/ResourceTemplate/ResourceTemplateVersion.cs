namespace Conductor.Engine.Domain.ResourceTemplate;

public sealed record ResourceTemplateVersion
{
    public required ResourceTemplateId TemplateId { get; init; }
    public required string Version { get; init; }
    public required ResourceTemplateVersionSource Source { get; init; }
    public required string Notes { get; init; }
    public required ResourceTemplateVersionState State { get; init; }
    public required DateTime CreatedAt { get; init; }

    private ResourceTemplateVersion()
    {
    }

    internal static ResourceTemplateVersion Create(CreateResourceTemplateVersionRequest request)
    {
        return new ResourceTemplateVersion
        {
            TemplateId = request.TemplateId,
            Version = request.Version,
            Source = request.Source,
            Notes = request.Notes,
            State = request.State,
            CreatedAt = request.CreatedAt,
        };
    }
}