namespace Conductor.Core.Environment;

public interface IEnvironmentRepository
{
    Task<Environment?> CreateAsync(Environment environment,
        CancellationToken cancellationToken = default);

    IEnumerable<Environment> GetAll();
    Task<Environment?> GetByIdAsync(EnvironmentId id, CancellationToken cancellationToken = default);
}