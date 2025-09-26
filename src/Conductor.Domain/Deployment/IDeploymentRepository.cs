using Conductor.Domain.Deployment.Domain;

namespace Conductor.Domain.Deployment;

public interface IDeploymentRepository
{
    Task<Domain.Deployment?> CreateAsync(Domain.Deployment deployment,
        CancellationToken cancellationToken = default);

    Task<Domain.Deployment?> GetByIdAsync(DeploymentId id, CancellationToken cancellationToken = default);
}