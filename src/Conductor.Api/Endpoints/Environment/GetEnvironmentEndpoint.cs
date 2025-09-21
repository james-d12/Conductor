using Conductor.Api.Common;
using Conductor.Core.Environment;
using Conductor.Core.Environment.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Conductor.Api.Endpoints.Environment;

public sealed class GetEnvironmentEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder) => builder
        .MapGet("/{id:guid}", HandleAsync)
        .WithSummary("Gets an environment by Id.");

    public sealed record GetEnvironmentResponse(Guid Id, string Name);

    private static async Task<Results<Ok<GetEnvironmentResponse>, NotFound>> HandleAsync(
        [FromQuery]
        Guid id,
        [FromServices]
        IEnvironmentRepository repository,
        CancellationToken cancellationToken)
    {
        var environmentId = new EnvironmentId(id);
        var environmentResponse = await repository.GetByIdAsync(environmentId, cancellationToken);

        if (environmentResponse is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(new GetEnvironmentResponse(environmentResponse.Id.Value, environmentResponse.Name));
    }
}