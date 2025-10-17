using Conductor.Engine.Api.Common;
using Conductor.Engine.Api.Endpoints.Application;
using Conductor.Engine.Api.Endpoints.Deployment;
using Conductor.Engine.Api.Endpoints.Environment;
using Conductor.Engine.Api.Endpoints.ResourceTemplate;

namespace Conductor.Engine.Api.Endpoints;

public static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapApplicationEndpoints();
        endpoints.MapEnvironmentEndpoints();
        endpoints.MapDeploymentEndpoints();
        endpoints.MapResourceTemplateEndpoints();
    }

    private static void MapApplicationEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/applications")
            .WithTags("Application");

        endpoints.MapPublicGroup()
            .MapEndpoint<CreateApplicationEndpoint>()
            .MapEndpoint<GetAllApplicationsEndpoint>()
            .MapEndpoint<GetApplicationEndpoint>();
    }

    private static void MapEnvironmentEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/environments")
            .WithTags("Environment");

        endpoints.MapPublicGroup()
            .MapEndpoint<CreateEnvironmentEndpoint>()
            .MapEndpoint<GetAllEnvironmentsEndpoint>()
            .MapEndpoint<GetEnvironmentEndpoint>();
    }

    private static void MapDeploymentEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/deployments")
            .WithTags("Deployment");

        endpoints.MapPublicGroup()
            .MapEndpoint<CreateDeploymentEndpoint>();
    }

    private static void MapResourceTemplateEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/resource-templates")
            .WithTags("Resource Template");

        endpoints.MapPublicGroup()
            .MapEndpoint<CreateResourceTemplateEndpoint>()
            .MapEndpoint<CreateResourceTemplateWithVersionEndpoint>()
            .MapEndpoint<GetResourceTemplateEndpoint>()
            .MapEndpoint<GetAllResourceTemplatesEndpoint>();
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