using Conductor.Engine.Api.Common;
using Conductor.Engine.Domain.Application;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Conductor.Engine.Api.Endpoints.Application;

public sealed class GetAllApplicationsEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder) => builder
        .MapGet("/", Handle)
        .WithSummary("Get All Applications.");

    private sealed record GetAllApplicationsResponse(List<GetApplicationEndpoint.GetApplicationResponse> Applications);

    private static Results<Ok<GetAllApplicationsResponse>, InternalServerError> Handle(
        [FromServices]
        IApplicationRepository repository)
    {
        var applications = repository.GetAll().ToList();
        var applicationsResponse = applications
            .Select(r => new GetApplicationEndpoint.GetApplicationResponse(r.Id.Value, r.Name))
            .ToList();
        return TypedResults.Ok(new GetAllApplicationsResponse(applicationsResponse));
    }
}