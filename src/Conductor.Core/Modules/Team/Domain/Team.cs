namespace Conductor.Core.Modules.Team.Domain;

public sealed record Team
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required List<User.Domain.User> Users { get; init; }
}