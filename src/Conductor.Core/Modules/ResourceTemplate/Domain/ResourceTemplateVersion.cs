using Conductor.Core.Modules.ResourceTemplate.Requests;

namespace Conductor.Core.Modules.ResourceTemplate.Domain;

public sealed record ResourceTemplateVersion
{
    public required ResourceTemplateId TemplateId { get; init; }
    public required string Version { get; init; }
    public required Uri Source { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string Notes { get; init; }

    public static ResourceTemplateVersion Create(CreateResourceTemplateVersionRequest request)
    {
        return new ResourceTemplateVersion
        {
            TemplateId = request.TemplateId,
            Version = request.Version,
            Source = request.Source,
            CreatedAt = request.CreatedAt,
            Notes = request.Notes
        };
    }
}