using Conductor.Domain.Environment.Domain;
using Conductor.Domain.ResourceTemplate.Domain;
using ApplicationId = Conductor.Domain.Application.Domain.ApplicationId;

namespace Conductor.Domain.Resource;

public sealed record Resource
{
    public required ResourceId Id { get; init; }
    public required string Name { get; init; }
    public required ResourceTemplateId ResourceTemplateId { get; init; }
    public required ApplicationId ApplicationId { get; init; }
    public required EnvironmentId EnvironmentId { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
}