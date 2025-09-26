using Conductor.Api.Common;
using Conductor.Domain.Application;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ApplicationId = Conductor.Domain.Application.Domain.ApplicationId;

namespace Conductor.Api.Endpoints.Application;

public sealed class GetApplicationEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder) => builder
        .MapGet("/{id:guid}", HandleAsync)
        .WithSummary("Gets an application by Id.");

    public sealed record GetApplicationResponse(Guid Id, string Name);

    private static async Task<Results<Ok<GetApplicationResponse>, NotFound>> HandleAsync(
        [FromQuery]
        Guid id,
        [FromServices]
        IApplicationRepository repository,
        CancellationToken cancellationToken)
    {
        var applicationId = new ApplicationId(id);
        var applicationResponse = await repository.GetByIdAsync(applicationId, cancellationToken);

        if (applicationResponse is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(new GetApplicationResponse(applicationResponse.Id.Value, applicationResponse.Name));
    }
}