using Conductor.Engine.Domain.Environment;
using Microsoft.EntityFrameworkCore;
using Environment = Conductor.Engine.Domain.Environment.Environment;

namespace Conductor.Engine.Persistence.Repositories;

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

    public IEnumerable<Environment> GetAll()
    {
        return _dbContext.Environments.AsEnumerable();
    }

    public Task<Environment?> GetByIdAsync(EnvironmentId id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Environments.FirstOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);
    }
}