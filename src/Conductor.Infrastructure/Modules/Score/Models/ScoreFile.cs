namespace Conductor.Infrastructure.Modules.Score.Models;

public sealed record ScoreFile
{
    // maps resource-name -> ScoreResource
    public required string ApiVersion { get; init; }
    public Dictionary<string, ScoreResource>? Resources { get; init; }
}