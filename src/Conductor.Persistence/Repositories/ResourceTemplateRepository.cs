using Conductor.Core.Modules.ResourceTemplate;
using Conductor.Core.Modules.ResourceTemplate.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Conductor.Persistence.Repositories;

public sealed class ResourceTemplateRepository : IResourceTemplateRepository
{
    private readonly ILogger<ResourceTemplateRepository> _logger;
    private readonly ConductorDbContext _dbContext;

    public ResourceTemplateRepository(ILogger<ResourceTemplateRepository> logger, ConductorDbContext dbContext)
    {
        _logger = logger;
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

    public async Task<ResourceTemplate?> GetByIdAsync(ResourceTemplateId id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.ResourceTemplates
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Could not get resource template");
            return null;
        }
    }

    public async Task<ResourceTemplate?> GetByTypeAsync(string type, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.ResourceTemplates
                .Where(r => r.Type == type)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Could not get resource template by type");
            return null;
        }
    }
}