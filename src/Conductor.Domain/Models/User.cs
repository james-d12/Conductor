namespace Conductor.Domain.Models;

public sealed record User
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string EmailAddress { get; init; }
}