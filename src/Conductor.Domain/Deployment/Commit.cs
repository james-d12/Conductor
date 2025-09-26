namespace Conductor.Domain.Deployment;

public sealed record Commit
{
    public required CommitId Id { get; init; }
    public required string Message { get; init; }
}