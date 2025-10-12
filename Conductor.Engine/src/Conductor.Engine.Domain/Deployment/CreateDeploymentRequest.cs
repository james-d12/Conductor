using Conductor.Engine.Domain.Environment;
using Application_ApplicationId = Conductor.Engine.Domain.Application.ApplicationId;
using ApplicationId = Conductor.Engine.Domain.Application.ApplicationId;

namespace Conductor.Engine.Domain.Deployment;

public sealed record CreateDeploymentRequest(
    Application_ApplicationId ApplicationId,
    EnvironmentId EnvironmentId,
    CommitId CommitId);