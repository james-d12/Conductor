using Conductor.Api.Common;
using Conductor.Core.Modules.Application;
using Conductor.Core.Modules.Deployment;
using Conductor.Core.Modules.Deployment.Requests;
using Conductor.Core.Modules.Environment;
using Conductor.Infrastructure.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Conductor.Api.Endpoints.Deployment;

public sealed class CreateDeployment : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder) => builder
        .MapPost("/", HandleAsync)
        .WithSummary("Creates a new deployment into an environment for an application with a given commit id.");

    private sealed record CreateDeploymentResponse(Guid Id, string Status, Uri Location);

    private static async Task<Results<Accepted<CreateDeploymentResponse>, BadRequest<string>, InternalServerError>>
        HandleAsync(
            CreateDeploymentRequest request,
            IDeploymentRepository repository,
            IApplicationRepository applicationRepository,
            IEnvironmentRepository environmentRepository,
            IResourceProvisioner resourceProvisioner,
            HttpContext httpContext,
            CancellationToken cancellationToken)
    {
        var application = await applicationRepository.GetByIdAsync(request.ApplicationId, cancellationToken);

        if (application is null)
        {
            return TypedResults.BadRequest($"Application with Id: {request.ApplicationId} does not exist.");
        }

        var environment = await environmentRepository.GetByIdAsync(request.EnvironmentId, cancellationToken);

        if (environment is null)
        {
            return TypedResults.BadRequest($"Environment with Id: {request.EnvironmentId} does not exist.");
        }

        var deployment = Core.Modules.Deployment.Domain.Deployment.Create(request);
        var deploymentResponse = await repository.CreateAsync(deployment, cancellationToken);

        if (deploymentResponse is null)
        {
            return TypedResults.InternalServerError();
        }

        await resourceProvisioner.StartAsync(application, deployment, cancellationToken);

        var locationUrl =
            new Uri(
                $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/deployments/{deploymentResponse.Id.Value}");

        var response = new CreateDeploymentResponse(
            Id: deploymentResponse.Id.Value,
            Status: "in_progress",
            Location: locationUrl
        );

        return TypedResults.Accepted(locationUrl, response);
    }
}