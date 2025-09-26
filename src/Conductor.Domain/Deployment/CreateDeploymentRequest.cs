using Conductor.Domain.Environment;
using ApplicationId = Conductor.Domain.Application.ApplicationId;

namespace Conductor.Domain.Deployment;

public sealed record CreateDeploymentRequest(
    ApplicationId ApplicationId,
    EnvironmentId EnvironmentId,
    CommitId CommitId);