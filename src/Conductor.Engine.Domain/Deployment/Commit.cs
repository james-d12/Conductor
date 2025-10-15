namespace Conductor.Engine.Domain.Deployment;

public sealed record Commit
{
    public required CommitId Id { get; init; }
    public required string Message { get; init; }

    private Commit()
    {
    }

    public static Commit Create(string commitHash, string message)
    {
        return new Commit
        {
            Id = new CommitId(commitHash),
            Message = message
        };
    }
}