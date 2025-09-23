namespace Conductor.Core.Provisioning.Requirements;

public record Requirement
{
    public required string Name { get; init; }
    public required List<RequirementResource> Resources { get; init; }
}