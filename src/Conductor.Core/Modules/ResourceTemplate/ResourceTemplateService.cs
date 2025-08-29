using Conductor.Core.Modules.ResourceTemplate.Requests;

namespace Conductor.Core.Modules.ResourceTemplate;

public sealed class ResourceTemplateService : IResourceTemplateService
{
    private readonly IResourceTemplateRepository _repository;

    public ResourceTemplateService(IResourceTemplateRepository repository)
    {
        _repository = repository;
    }

    public async Task<Domain.ResourceTemplate?> CreateAsync(CreateResourceTemplateRequest createResourceTemplateRequest,
        CancellationToken cancellationToken = default)
    {
        var resourceTemplateDomain = Domain.ResourceTemplate.Create(createResourceTemplateRequest);
        Domain.ResourceTemplate? resourceTemplatePersistence =
            await _repository.CreateAsync(resourceTemplateDomain, cancellationToken);
        return resourceTemplatePersistence;
    }
}