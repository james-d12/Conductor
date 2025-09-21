using Conductor.Api.Common;
using Conductor.Core.Application;
using Conductor.Core.Application.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Conductor.Api.Endpoints.Application;

public sealed class CreateApplicationEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder) => builder
        .MapPost("/", HandleAsync)
        .WithSummary("Creates a new application.");

    private sealed record CreateApplicationResponse(Guid Id);

    private static async Task<Results<Ok<CreateApplicationResponse>, InternalServerError>> HandleAsync(
        [FromBody]
        CreateApplicationRequest request,
        [FromServices]
        IApplicationRepository repository,
        CancellationToken cancellationToken)
    {
        var application = Core.Application.Domain.Application.Create(request);
        var applicationResponse = await repository.CreateAsync(application, cancellationToken);

        if (applicationResponse is null)
        {
            return TypedResults.InternalServerError();
        }

        return TypedResults.Ok(new CreateApplicationResponse(applicationResponse.Id.Value));
    }
}