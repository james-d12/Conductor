namespace Conductor.Core.Deployment.Domain;

public readonly record struct CommitId(string Value);

public sealed record Commit
{
    public required CommitId Id { get; init; }
    public required string Message { get; init; }
}