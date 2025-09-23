namespace Conductor.Core.Provisioning.Requirements;

public sealed record RequirementResult
{
    public required Requirement? Requirement { get; init; }
}