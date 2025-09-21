using Conductor.Core.ResourceTemplate;
using Conductor.Core.ResourceTemplate.Domain;
using Microsoft.EntityFrameworkCore;

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
        var result = await _dbContext.ResourceTemplates.AddAsync(resourceTemplate, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public IEnumerable<ResourceTemplate> GetAll()
    {
        return _dbContext.ResourceTemplates.AsEnumerable();
    }

    public Task<ResourceTemplate?> GetByIdAsync(ResourceTemplateId id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.ResourceTemplates.FirstOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);
    }

    public Task<ResourceTemplate?> GetByTypeAsync(string type, CancellationToken cancellationToken = default)
    {
        return _dbContext.ResourceTemplates.FirstOrDefaultAsync(t => t.Type == type, cancellationToken);
    }
}