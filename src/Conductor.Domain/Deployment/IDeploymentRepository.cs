namespace Conductor.Domain.Deployment;

public interface IDeploymentRepository
{
    Task<Deployment?> CreateAsync(Deployment deployment,
        CancellationToken cancellationToken = default);

    Task<Deployment?> GetByIdAsync(DeploymentId id, CancellationToken cancellationToken = default);
}