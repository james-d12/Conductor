using Conductor.Api.Common;
using Conductor.Core.Modules.Environment;
using Conductor.Core.Modules.Environment.Requests;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Conductor.Api.Endpoints.Environment;

public sealed class CreateEnvironment : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder) => builder
        .MapPost("/", HandleAsync)
        .WithSummary("Creates a new environment.");

    private sealed record CreateEnvironmentResponse(Guid Id);

    private static async Task<Results<Ok<CreateEnvironmentResponse>, InternalServerError>> HandleAsync(
        CreateEnvironmentRequest request,
        IEnvironmentRepository repository,
        CancellationToken cancellationToken)
    {
        var environment = Core.Modules.Environment.Domain.Environment.Create(request);
        var environmentResponse = await repository.CreateAsync(environment, cancellationToken);

        if (environmentResponse is null)
        {
            return TypedResults.InternalServerError();
        }

        return TypedResults.Ok(new CreateEnvironmentResponse(environmentResponse.Id.Value));
    }
}