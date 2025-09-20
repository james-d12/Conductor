using Conductor.Core.Modules.Application;
using Conductor.Core.Modules.Application.Domain;

namespace Conductor.Persistence.Repositories;

public sealed class ApplicationRepository : IApplicationRepository
{
    private readonly ConductorDbContext _dbContext;

    public ApplicationRepository(ConductorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Application?> CreateAsync(Application application,
        CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Applications.AddAsync(application, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }
}