namespace Conductor.Core.Modules.ResourceTemplate;

public interface IResourceTemplateRepository
{
    Task<Domain.ResourceTemplate?> CreateAsync(Domain.ResourceTemplate resourceTemplate,
        CancellationToken cancellationToken = default);
}