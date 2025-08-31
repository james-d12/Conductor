namespace Conductor.Infrastructure.Common;

public sealed record LocalFile
{
    public required string Name { get; init; }
    public required string Directory { get; init; }
    public required string FullPath { get; init; }
}