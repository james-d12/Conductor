using Conductor.Domain.Models.Resource;
using Conductor.Domain.Models.ResourceTemplate;

namespace Conductor.Domain.Models.Environment;

public sealed record EnvironmentResource : Resource.Resource
{
    public required EnvironmentId EnvironmentId { get; init; }

    private EnvironmentResource()
    {
    }

    public static EnvironmentResource Create(
        string name,
        ResourceTemplateVersion templateVersion,
        EnvironmentId environmentId)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(templateVersion);

        return new EnvironmentResource
        {
            Id = new ResourceId(),
            Name = name,
            TemplateVersion = templateVersion,
            EnvironmentId = environmentId
        };
    }
}