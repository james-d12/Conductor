using Conductor.Core.Modules.Environment;
using Environment = Conductor.Core.Modules.Environment.Domain.Environment;

namespace Conductor.Persistence.Repositories;

public sealed class EnvironmentRepository : IEnvironmentRepository
{
    private readonly ConductorDbContext _dbContext;

    public EnvironmentRepository(ConductorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Environment?> CreateAsync(Environment application,
        CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Environments.AddAsync(application, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }
}