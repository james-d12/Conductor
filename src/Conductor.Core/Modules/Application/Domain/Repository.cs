namespace Conductor.Core.Modules.Application.Domain;

public sealed record Repository
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required Uri Url { get; init; }
    public required RepositoryProvider Provider { get; init; }
}