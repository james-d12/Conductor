using Conductor.Domain.Application;
using Microsoft.EntityFrameworkCore;
using ApplicationId = Conductor.Domain.Application.ApplicationId;

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

    public IEnumerable<Application> GetAll()
    {
        return _dbContext.Applications.AsEnumerable();
    }

    public Task<Application?> GetByIdAsync(ApplicationId id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Applications.FirstOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);
    }
}