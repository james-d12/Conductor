using Conductor.Domain.Models.Resource;
using Conductor.Domain.Models.ResourceTemplate;

namespace Conductor.Domain.Models.Application;

public sealed record ApplicationResource : Resource.Resource
{
    public required ApplicationId ApplicationId { get; init; }

    private ApplicationResource()
    {
    }

    public static ApplicationResource Create(
        string name,
        ResourceTemplateVersion templateVersion,
        ApplicationId applicationId)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(templateVersion);

        return new ApplicationResource
        {
            Id = new ResourceId(),
            Name = name,
            TemplateVersion = templateVersion,
            ApplicationId = applicationId
        };
    }
}