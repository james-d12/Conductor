using Conductor.Core.Modules.Deployment;
using Conductor.Core.Modules.Deployment.Domain;

namespace Conductor.Persistence.Repositories;

public sealed class DeploymentRepository : IDeploymentRepository
{
    private readonly ConductorDbContext _dbContext;

    public DeploymentRepository(ConductorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Deployment?> CreateAsync(Deployment deployment,
        CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Deployments.AddAsync(deployment, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }
}