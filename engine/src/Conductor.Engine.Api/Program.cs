using Conductor.Engine.Api.Common;
using Conductor.Engine.Api.Endpoints;
using Conductor.Engine.Infrastructure;
using Conductor.Engine.Persistence;
using Microsoft.AspNetCore.Identity;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

builder.Services.AddLogging();
builder.Services.AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection("JwtOptions"))
    .ValidateDataAnnotations()
    .ValidateOnStart();


builder.Services
    .AddOpenApi()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddPersistenceServices()
    .AddInfrastructureServices();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ConductorDbContext>();

await builder.Services.ApplyMigrations();

WebApplication app = builder.Build();

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.MapEndpoints();

await app.RunAsync();