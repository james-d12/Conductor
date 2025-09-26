using Conductor.Api.Common;
using Conductor.Domain.Environment;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Conductor.Api.Endpoints.Environment;

public sealed class CreateEnvironmentEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder) => builder
        .MapPost("/", HandleAsync)
        .WithSummary("Creates a new environment.");

    private sealed record CreateEnvironmentResponse(Guid Id);

    private static async Task<Results<Ok<CreateEnvironmentResponse>, InternalServerError>> HandleAsync(
        [FromBody]
        CreateEnvironmentRequest request,
        [FromServices]
        IEnvironmentRepository repository,
        CancellationToken cancellationToken)
    {
        var environment = Domain.Environment.Environment.Create(request);
        var environmentResponse = await repository.CreateAsync(environment, cancellationToken);

        if (environmentResponse is null)
        {
            return TypedResults.InternalServerError();
        }

        return TypedResults.Ok(new CreateEnvironmentResponse(environmentResponse.Id.Value));
    }
}