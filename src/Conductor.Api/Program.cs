using Conductor.Api.Endpoints;
using Conductor.Core;
using Conductor.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCoreServices();
builder.Services.AddPersistenceServices();

await builder.Services.ApplyMigrations();

var app = builder.Build();

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.MapEndpoints();

await app.RunAsync();