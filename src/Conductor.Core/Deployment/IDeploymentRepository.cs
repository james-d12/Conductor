using Conductor.Core.Deployment.Domain;

namespace Conductor.Core.Deployment;

public interface IDeploymentRepository
{
    Task<Domain.Deployment?> CreateAsync(Domain.Deployment deployment,
        CancellationToken cancellationToken = default);

    Task<Domain.Deployment?> GetByIdAsync(DeploymentId id, CancellationToken cancellationToken = default);
}