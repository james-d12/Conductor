namespace Conductor.Core.Modules.Application;

public interface IApplicationRepository
{
    Task<Domain.Application?> CreateAsync(Domain.Application application,
        CancellationToken cancellationToken = default);
}