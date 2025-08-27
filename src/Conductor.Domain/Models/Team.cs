namespace Conductor.Domain.Models;

public sealed record Team
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required List<User> Users { get; init; }
}