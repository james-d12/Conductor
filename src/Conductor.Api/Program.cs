using Conductor.Api.Endpoints;
using Conductor.Infrastructure;
using Conductor.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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