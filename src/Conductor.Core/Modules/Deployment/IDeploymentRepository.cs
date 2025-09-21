namespace Conductor.Core.Modules.Deployment;

public interface IDeploymentRepository
{
    Task<Domain.Deployment?> CreateAsync(Domain.Deployment deployment,
        CancellationToken cancellationToken = default);
}