namespace Conductor.Core.Modules.Environment;

public interface IEnvironmentRepository
{
    Task<Domain.Environment?> CreateAsync(Domain.Environment environment,
        CancellationToken cancellationToken = default);
}