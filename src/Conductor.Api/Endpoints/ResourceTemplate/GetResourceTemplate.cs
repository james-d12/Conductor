using Conductor.Api.Common;
using Conductor.Core.Modules.ResourceTemplate;
using Conductor.Core.Modules.ResourceTemplate.Domain;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Conductor.Api.Endpoints.ResourceTemplate;

public sealed class GetResourceTemplate : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder) => builder
        .MapGet("/{id:guid}", HandleAsync)
        .WithSummary("Gets an existing resource template by id.");

    private sealed record GetResourceTemplateResponse(string Name);

    private static async Task<Results<Ok<GetResourceTemplateResponse>, NotFound, InternalServerError>> HandleAsync(
        Guid id,
        IResourceTemplateRepository repository,
        ILogger<GetResourceTemplate> logger,
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