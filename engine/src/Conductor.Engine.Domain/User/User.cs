namespace Conductor.Engine.Domain.User;

public sealed record User
{
    public required UserId Id { get; init; }
    public required string FirstName { get; init; } = string.Empty;
    public required string LastName { get; init; } = string.Empty;
    public required string Email { get; init; } = string.Empty;
}