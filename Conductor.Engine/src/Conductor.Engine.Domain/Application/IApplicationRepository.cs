namespace Conductor.Engine.Domain.Application;

using ApplicationId = ApplicationId;

public interface IApplicationRepository
{
    Task<Application?> CreateAsync(Application application,
        CancellationToken cancellationToken = default);

    IEnumerable<Application> GetAll();
    Task<Application?> GetByIdAsync(Domain.Application.ApplicationId id, CancellationToken cancellationToken = default);
}