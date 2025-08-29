using Conductor.Core.Modules.ResourceTemplate.Requests;

namespace Conductor.Core.Modules.ResourceTemplate;

public interface IResourceTemplateService
{
    Task<Domain.ResourceTemplate?> CreateAsync(CreateResourceTemplateRequest createResourceTemplateRequest,
        CancellationToken cancellationToken = default);
}