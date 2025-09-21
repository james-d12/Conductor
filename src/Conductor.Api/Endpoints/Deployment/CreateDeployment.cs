using Conductor.Api.Common;
using Conductor.Core.Modules.Deployment;
using Conductor.Core.Modules.Deployment.Requests;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Conductor.Api.Endpoints.Deployment;

public sealed class CreateDeployment : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder) => builder
        .MapPost("/", HandleAsync)
        .WithSummary("Creates a new deployment into an environment for an application with a given commit id.");

    private sealed record CreateDeploymentResponse(Guid Id, string Status, Uri Location);

    private static async Task<Results<Accepted<CreateDeploymentResponse>, InternalServerError>> HandleAsync(
        CreateDeploymentRequest request,
        IDeploymentRepository repository,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var deployment = Core.Modules.Deployment.Domain.Deployment.Create(request);
        var deploymentResponse = await repository.CreateAsync(deployment, cancellationToken);

        if (deploymentResponse is null)
        {
            return TypedResults.InternalServerError();
        }
        
        var locationUrl = new Uri($"{httpContext.Request.Scheme}://{httpContext.Request.Host}/deployments/{deploymentResponse.Id.Value}");

        var response = new CreateDeploymentResponse(
            Id: deploymentResponse.Id.Value,
            Status: "in_progress",
            Location: locationUrl
        );

        return TypedResults.Accepted(locationUrl, response);
    }
}