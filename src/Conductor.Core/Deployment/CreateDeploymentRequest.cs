using Conductor.Core.Environment;
using ApplicationId = Conductor.Core.Application.ApplicationId;

namespace Conductor.Core.Deployment;

public sealed record CreateDeploymentRequest(
    ApplicationId ApplicationId,
    EnvironmentId EnvironmentId,
    CommitId CommitId);