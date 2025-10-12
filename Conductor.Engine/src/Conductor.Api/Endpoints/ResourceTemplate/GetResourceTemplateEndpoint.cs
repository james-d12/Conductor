using Conductor.Api.Common;
using Conductor.Domain.ResourceTemplate;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Conductor.Api.Endpoints.ResourceTemplate;

public sealed class GetResourceTemplateEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder) => builder
        .MapGet("/{id:guid}", HandleAsync)
        .WithSummary("Gets an existing resource template by id.");

    public sealed record GetResourceTemplateResponse(string Name);

    private static async Task<Results<Ok<GetResourceTemplateResponse>, NotFound, InternalServerError>> HandleAsync(
        [FromQuery]
        Guid id,
        [FromServices]
        IResourceTemplateRepository repository,
        [FromServices]
        ILogger<GetResourceTemplateEndpoint> logger,
        CancellationToken cancellationToken)
    {
        try
        {
            var resourceTemplateId = new ResourceTemplateId(id);
            var resourceTemplateResponse = await repository.GetByIdAsync(resourceTemplateId, cancellationToken);

            if (resourceTemplateResponse is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(new GetResourceTemplateResponse(resourceTemplateResponse.Name));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Could not retrieve Resource Template for {Id}.", id);
            return TypedResults.InternalServerError();
        }
    }
}