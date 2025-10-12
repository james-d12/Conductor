namespace Conductor.Engine.Domain.Organisation;

public sealed record Organisation
{
    public required OrganisationId Id { get; init; }
    public required string Name { get; init; } = string.Empty;
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }

    private Organisation()
    {
    }

    public static Organisation Create(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        return new Organisation
        {
            Id = new OrganisationId(),
            Name = name,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }
}