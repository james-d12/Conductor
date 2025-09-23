namespace Conductor.Core.Application;

public interface IApplicationRepository
{
    Task<Application?> CreateAsync(Application application,
        CancellationToken cancellationToken = default);

    IEnumerable<Application> GetAll();
    Task<Application?> GetByIdAsync(ApplicationId id, CancellationToken cancellationToken = default);
}