using Conductor.Api.Common;
using Conductor.Core.Modules.ResourceTemplate;
using Conductor.Core.Modules.ResourceTemplate.Requests;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Conductor.Api.Endpoints.ResourceTemplate;

public sealed class CreateResourceTemplate : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder) => builder
        .MapPost("/", HandleAsync)
        .WithSummary("Creates a new resource template.");

    private sealed record CreateResourceTemplateResponse(Guid Id);

    private static async Task<Results<Ok<CreateResourceTemplateResponse>, InternalServerError>> HandleAsync(
        CreateResourceTemplateRequest request,
        IResourceTemplateRepository repository,
        CancellationToken cancellationToken)
    {
        var resourceTemplate = Core.Modules.ResourceTemplate.Domain.ResourceTemplate.Create(request);
        var resourceTemplateResponse = await repository.CreateAsync(resourceTemplate, cancellationToken);

        if (resourceTemplateResponse is null)
        {
            return TypedResults.InternalServerError();
        }

        return TypedResults.Ok(new CreateResourceTemplateResponse(resourceTemplateResponse.Id.Value));
    }
}