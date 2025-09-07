using Conductor.Core.Modules.ResourceTemplate.Domain;

namespace Conductor.Core.Modules.ResourceTemplate;

public interface IResourceTemplateRepository
{
    Task<Domain.ResourceTemplate?> CreateAsync(Domain.ResourceTemplate resourceTemplate,
        CancellationToken cancellationToken = default);

    IEnumerable<Domain.ResourceTemplate> GetAll();
    Task<Domain.ResourceTemplate?> GetByIdAsync(ResourceTemplateId id, CancellationToken cancellationToken = default);
    Task<Domain.ResourceTemplate?> GetByTypeAsync(string type, CancellationToken cancellationToken = default);
}