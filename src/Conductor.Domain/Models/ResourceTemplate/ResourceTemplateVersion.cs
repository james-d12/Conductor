using Conductor.Domain.Models.ResourceTemplate.Requests;

namespace Conductor.Domain.Models.ResourceTemplate;

public sealed record ResourceTemplateVersion
{
    public required ResourceTemplateId TemplateId { get; init; }
    public required string Version { get; init; }
    public required Uri Source { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string Notes { get; init; }
    public required Dictionary<string, string> Inputs { get; init; } = new();
    public required Dictionary<string, string> Outputs { get; init; } = new();

    public static ResourceTemplateVersion Create(CreateResourceTemplateVersionRequest request)
    {
        return new ResourceTemplateVersion
        {
            TemplateId = request.TemplateId,
            Version = request.Version,
            Source = request.Source,
            CreatedAt = request.CreatedAt,
            Notes = request.Notes,
            Inputs = request.Inputs,
            Outputs = request.Outputs
        };
    }
}