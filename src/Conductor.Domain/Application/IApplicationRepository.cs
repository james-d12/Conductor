using ApplicationId = Conductor.Domain.Application.Domain.ApplicationId;

namespace Conductor.Domain.Application;

using ApplicationId = Domain.ApplicationId;

public interface IApplicationRepository
{
    Task<Domain.Application?> CreateAsync(Domain.Application application,
        CancellationToken cancellationToken = default);

    IEnumerable<Domain.Application> GetAll();
    Task<Domain.Application?> GetByIdAsync(ApplicationId id, CancellationToken cancellationToken = default);
}