using Conductor.Api.Common;
using Conductor.Domain.ResourceTemplate;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Conductor.Api.Endpoints.ResourceTemplate;

public sealed class CreateResourceTemplateWithVersionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder) => builder
        .MapPost("/with-version", HandleAsync)
        .WithSummary("Creates a new resource template with the specified version");

    private sealed record CreateResourceTemplateWithVersionResponse(Guid Id);

    private static async Task<Results<Ok<CreateResourceTemplateWithVersionResponse>, InternalServerError>> HandleAsync(
        [FromBody]
        CreateResourceTemplateWithVersionRequest request,
        [FromServices]
        IResourceTemplateRepository repository,
        CancellationToken cancellationToken)
    {
        var resourceTemplate = Domain.ResourceTemplate.ResourceTemplate.CreateWithVersion(request);
        var resourceTemplateResponse = await repository.CreateAsync(resourceTemplate, cancellationToken);

        if (resourceTemplateResponse is null)
        {
            return TypedResults.InternalServerError();
        }

        return TypedResults.Ok(new CreateResourceTemplateWithVersionResponse(resourceTemplateResponse.Id.Value));
    }
}