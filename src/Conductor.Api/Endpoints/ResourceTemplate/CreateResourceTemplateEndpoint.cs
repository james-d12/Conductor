using Conductor.Api.Common;
using Conductor.Core.ResourceTemplate;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Conductor.Api.Endpoints.ResourceTemplate;

public sealed class CreateResourceTemplateEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder) => builder
        .MapPost("/", HandleAsync)
        .WithSummary("Creates a new resource template.");

    public sealed record CreateResourceTemplateRequest
    {
        public required string Name { get; init; }
        public required string Type { get; init; }
        public required string Description { get; init; }
        public required ResourceTemplateProvider Provider { get; init; }
    };

    private sealed record CreateResourceTemplateResponse(Guid Id);

    private static async Task<Results<Ok<CreateResourceTemplateResponse>, InternalServerError>> HandleAsync(
        [FromBody]
        CreateResourceTemplateRequest request,
        [FromServices]
        IResourceTemplateRepository repository,
        CancellationToken cancellationToken)
    {
        var resourceTemplate = Core.ResourceTemplate.ResourceTemplate.Create(
            name: request.Name,
            type: request.Type,
            description: request.Description,
            provider: request.Provider);
        
        var resourceTemplateResponse = await repository.CreateAsync(resourceTemplate, cancellationToken);

        if (resourceTemplateResponse is null)
        {
            return TypedResults.InternalServerError();
        }

        return TypedResults.Ok(new CreateResourceTemplateResponse(resourceTemplateResponse.Id.Value));
    }
}