using Conductor.Core.Environment.Domain;

namespace Conductor.Core.Environment;

public interface IEnvironmentRepository
{
    Task<Domain.Environment?> CreateAsync(Domain.Environment environment,
        CancellationToken cancellationToken = default);

    IEnumerable<Domain.Environment> GetAll();
    Task<Domain.Environment?> GetByIdAsync(EnvironmentId id, CancellationToken cancellationToken = default);
}