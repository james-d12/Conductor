using Conductor.Api.Common;
using Conductor.Core.ResourceTemplate;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Conductor.Api.Endpoints.ResourceTemplate;

public sealed class GetAllResourceTemplates : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder) => builder
        .MapGet("/", Handle)
        .WithSummary("Gets all resource templates.");

    private sealed record GetAllResourceTemplatesResponse(
        List<GetResourceTemplate.GetResourceTemplateResponse> ResourceTemplates);

    private static Results<Ok<GetAllResourceTemplatesResponse>, NotFound, InternalServerError> Handle(
        IResourceTemplateRepository repository,
        ILogger<GetResourceTemplate> logger,
        CancellationToken cancellationToken)
    {
        try
        {
            var resourceTemplates = repository.GetAll().ToList();
            var resourceTemplatesResponse = resourceTemplates
                .Select(r => new GetResourceTemplate.GetResourceTemplateResponse(r.Name))
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