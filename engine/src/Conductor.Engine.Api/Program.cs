using Conductor.Engine.Api.Endpoints;
using Conductor.Engine.Infrastructure;
using Conductor.Engine.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

builder.Services
    .AddOpenApi()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddPersistenceServices()
    .AddInfrastructureServices();

await builder.Services.ApplyMigrations();

WebApplication app = builder.Build();

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.MapEndpoints();

await app.RunAsync();