using ApplicationId = Conductor.Core.Application.Domain.ApplicationId;

namespace Conductor.Core.Application;

public interface IApplicationRepository
{
    Task<Domain.Application?> CreateAsync(Domain.Application application,
        CancellationToken cancellationToken = default);

    IEnumerable<Domain.Application> GetAll();
    Task<Domain.Application?> GetByIdAsync(ApplicationId id, CancellationToken cancellationToken = default);
}