using Conductor.Api.Common;
using Conductor.Api.Endpoints.Application;
using Conductor.Api.Endpoints.ResourceTemplate;

namespace Conductor.Api.Endpoints;

public static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapApplicationEndpoints();
        endpoints.MapResourceTemplateEndpoints();
    }

    private static void MapApplicationEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/application")
            .WithTags("Application");

        endpoints.MapPublicGroup()
            .MapEndpoint<CreateApplication>();
    }

    private static void MapResourceTemplateEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/resource-template")
            .WithTags("Resource Template");

        endpoints.MapPublicGroup()
            .MapEndpoint<CreateResourceTemplate>()
            .MapEndpoint<CreateResourceTemplateWithVersion>();
    }

    private static RouteGroupBuilder MapPublicGroup(this IEndpointRouteBuilder app, string? prefix = null)
    {
        return app.MapGroup(prefix ?? string.Empty)
            .AllowAnonymous();
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}