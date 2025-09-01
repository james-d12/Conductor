using Conductor.Core.Modules.ResourceTemplate.Domain;

namespace Conductor.Core.Modules.ResourceTemplate.Requests;

public sealed record CreateNewResourceTemplateVersionRequest
{
    public required string Version { get; init; }
    public required Uri Source { get; init; }
    public required string Notes { get; init; }
    public required ResourceTemplateVersionState State { get; init; }
}