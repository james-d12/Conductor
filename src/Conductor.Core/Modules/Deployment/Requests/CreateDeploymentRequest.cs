using Conductor.Core.Modules.Deployment.Domain;
using Conductor.Core.Modules.Environment.Domain;
using ApplicationId = Conductor.Core.Modules.Application.Domain.ApplicationId;

namespace Conductor.Core.Modules.Deployment.Requests;

public sealed record CreateDeploymentRequest(
    ApplicationId ApplicationId,
    EnvironmentId EnvironmentId,
    CommitId CommitId);