namespace Conductor.Domain.ResourceTemplate;

public interface IResourceTemplateRepository
{
    Task<ResourceTemplate?> CreateAsync(ResourceTemplate resourceTemplate,
        CancellationToken cancellationToken = default);

    IEnumerable<ResourceTemplate> GetAll();
    Task<ResourceTemplate?> GetByIdAsync(ResourceTemplateId id, CancellationToken cancellationToken = default);
    Task<ResourceTemplate?> GetByTypeAsync(string type, CancellationToken cancellationToken = default);
}