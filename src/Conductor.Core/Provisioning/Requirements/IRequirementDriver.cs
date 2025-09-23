namespace Conductor.Core.Provisioning.Requirements;

public interface IRequirementDriver
{
    Task<RequirementResult> GetRequirementsAsync(
        Application.Application application,
        Deployment.Deployment deployment,
        CancellationToken cancellationToken = default);
}