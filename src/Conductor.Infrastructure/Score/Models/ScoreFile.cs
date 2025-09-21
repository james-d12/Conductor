namespace Conductor.Infrastructure.Score.Models;

public sealed record ScoreFile
{
    // maps resource-name -> ScoreResource
    public required string ApiVersion { get; init; }
    public required ScoreMetadata Metadata { get; init; }
    public Dictionary<string, ScoreResource>? Resources { get; init; }
}