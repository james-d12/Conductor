using Conductor.Domain.Deployment.Domain;
using Conductor.Domain.Environment.Domain;
using ApplicationId = Conductor.Domain.Application.Domain.ApplicationId;

namespace Conductor.Domain.Deployment.Requests;

public sealed record CreateDeploymentRequest(
    ApplicationId ApplicationId,
    EnvironmentId EnvironmentId,
    CommitId CommitId);