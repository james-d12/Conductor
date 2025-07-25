namespace Conductor.Domain.Models.ResourceTemplate.Requests;

public sealed record CreateNewResourceTemplateVersionRequest
{
    public required string Version { get; init; }
    public required Uri Source { get; init; }
    public required string Notes { get; init; }
    public required Dictionary<string, string> Inputs { get; init; }
    public required Dictionary<string, string> Outputs { get; init; }
}