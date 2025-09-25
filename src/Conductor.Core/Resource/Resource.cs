using Conductor.Core.Environment.Domain;
using Conductor.Core.ResourceTemplate.Domain;
using ApplicationId = Conductor.Core.Application.Domain.ApplicationId;

namespace Conductor.Core.Resource;

public sealed record Resource
{
    public required ResourceId Id { get; init; }
    public required ResourceTemplateId ResourceTemplateId { get; init; }
    public required ApplicationId ApplicationId { get; init; }
    public required EnvironmentId EnvironmentId { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
}