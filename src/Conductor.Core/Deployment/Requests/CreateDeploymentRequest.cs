using Conductor.Core.Deployment.Domain;
using Conductor.Core.Environment.Domain;
using ApplicationId = Conductor.Core.Application.Domain.ApplicationId;

namespace Conductor.Core.Deployment.Requests;

public sealed record CreateDeploymentRequest(
    ApplicationId ApplicationId,
    EnvironmentId EnvironmentId,
    CommitId CommitId);