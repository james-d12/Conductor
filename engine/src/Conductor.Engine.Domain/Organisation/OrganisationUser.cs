namespace Conductor.Engine.Domain.Organisation;

public sealed record OrganisationUser
{
    public required string IdentityUserId { get; init; }
    public required OrganisationId OrganisationId { get; init; }
}