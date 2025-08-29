using Conductor.Core.Modules.ResourceTemplate;
using Conductor.Core.Modules.ResourceTemplate.Domain;

namespace Conductor.Persistence.Repositories;

public sealed class ResourceTemplateRepository : IResourceTemplateRepository
{
    private readonly ConductorDbContext _dbContext;

    public ResourceTemplateRepository(ConductorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResourceTemplate?> CreateAsync(ResourceTemplate resourceTemplate,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _dbContext.ResourceTemplates.AddAsync(resourceTemplate, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return null;
        }
    }
}