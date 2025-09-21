using Conductor.Core.Modules.Deployment.Domain;

namespace Conductor.Core.Modules.Deployment;

public interface IDeploymentRepository
{
    Task<Domain.Deployment?> CreateAsync(Domain.Deployment deployment,
        CancellationToken cancellationToken = default);

    Task<Domain.Deployment?> GetByIdAsync(DeploymentId id, CancellationToken cancellationToken = default);
}