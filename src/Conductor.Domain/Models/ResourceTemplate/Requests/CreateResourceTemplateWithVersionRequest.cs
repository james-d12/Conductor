namespace Conductor.Domain.Models.ResourceTemplate.Requests;

public sealed record CreateResourceTemplateWithVersionRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required ResourceTemplateProvider Provider { get; init; }
    public required ResourceTemplateType Type { get; init; }
    public required string Version { get; init; }
    public required Uri Source { get; init; }
    public required string Notes { get; init; }
    public required Dictionary<string, string> Inputs { get; init; }
    public required Dictionary<string, string> Outputs { get; init; }
}