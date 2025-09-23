using Conductor.Api.Common;
using Conductor.Core.ResourceTemplate;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Conductor.Api.Endpoints.ResourceTemplate;

public sealed class CreateResourceTemplateWithVersionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder) => builder
        .MapPost("/with-version", HandleAsync)
        .WithSummary("Creates a new resource template with the specified version");

    public sealed record CreateResourceTemplateWithVersionRequest
    {
        public required string Name { get; init; }
        public required string Type { get; init; }
        public required string Description { get; init; }
        public required ResourceTemplateProvider Provider { get; init; }
        public required string Version { get; init; }
        public required ResourceTemplateVersionSource Source { get; init; }
        public required string Notes { get; init; }
        public required ResourceTemplateVersionState State { get; init; }
    }

    private sealed record CreateResourceTemplateWithVersionResponse(Guid Id);

    private static async Task<Results<Ok<CreateResourceTemplateWithVersionResponse>, InternalServerError>> HandleAsync(
        [FromBody]
        CreateResourceTemplateWithVersionRequest request,
        [FromServices]
        IResourceTemplateRepository repository,
        CancellationToken cancellationToken)
    {
        var resourceTemplate = Core.ResourceTemplate.ResourceTemplate.Create(
            name: request.Name,
            type: request.Type,
            description: request.Description,
            provider: request.Provider);

        resourceTemplate.AddVersion(
            version: request.Version,
            source: request.Source,
            notes: request.Notes,
            state: request.State);

        var resourceTemplateResponse = await repository.CreateAsync(resourceTemplate, cancellationToken);

        if (resourceTemplateResponse is null)
        {
            return TypedResults.InternalServerError();
        }

        return TypedResults.Ok(new CreateResourceTemplateWithVersionResponse(resourceTemplateResponse.Id.Value));
    }
}