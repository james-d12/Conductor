namespace Conductor.Domain.Models;

public readonly record struct CommitId(string Id);

public sealed record Commit
{
    public required CommitId Id { get; init; }
    public required string Message { get; init; }
}