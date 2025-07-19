namespace Conductor.Domain.Models;

public sealed record Owner
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string EmailAddress { get; init; }
}