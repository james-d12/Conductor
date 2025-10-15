using Conductor.Engine.Api.Common;
using Conductor.Engine.Domain.ResourceTemplate;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Conductor.Engine.Api.Endpoints.ResourceTemplate;

public sealed class GetAllResourceTemplatesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder) => builder
        .MapGet("/", Handle)
        .WithSummary("Gets all resource templates.");

    private sealed record GetAllResourceTemplatesResponse(
        List<GetResourceTemplateEndpoint.GetResourceTemplateResponse> ResourceTemplates);

    private static Results<Ok<GetAllResourceTemplatesResponse>, NotFound, InternalServerError> Handle(
        [FromServices]
        IResourceTemplateRepository repository,
        [FromServices]
        ILogger<GetResourceTemplateEndpoint> logger,
        CancellationToken cancellationToken)
    {
        try
        {
            var resourceTemplates = repository.GetAll().ToList();
            var resourceTemplatesResponse = resourceTemplates
                .Select(r => new GetResourceTemplateEndpoint.GetResourceTemplateResponse(r.Name))
                .ToList();

            return TypedResults.Ok(new GetAllResourceTemplatesResponse(resourceTemplatesResponse));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Could not retrieve All Resource Templates");
            return TypedResults.InternalServerError();
        }
    }
}