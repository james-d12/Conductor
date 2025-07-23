using Conductor.Domain.Models.ResourceTemplate;
using Conductor.Domain.Models.ResourceTemplate.Requests;
using Conductor.Domain.Services;
using Conductor.Infrastructure;
using Conductor.Infrastructure.Drivers.Terraform;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder();

builder.Configuration.AddUserSecrets<Program>();

builder.Logging.ClearProviders();
builder.Logging.AddJsonConsole();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Services.AddInfrastructureServices();

builder.Services.AddLogging();

using var host = builder.Build();

var cosmosDbResourceTemplate = ResourceTemplate.CreateWithVersion(new CreateResourceTemplateWithVersionRequest
{
    Name = "Cosmos Db",
    Description = "The Cosmos Db Terraform Resource Template",
    Provider = ResourceTemplateProvider.Terraform,
    Type = ResourceTemplateType.AzureCosmosDb,
    Version = "1.0.0",
    Source = new Uri("https://github.com/terraform/cosmosdb?tag=v1.0.0"),
    Notes = "",
    Inputs = [],
    Outputs = []
});

cosmosDbResourceTemplate.AddVersion(new CreateNewResourceTemplateVersionRequest
{
    Version = "1.1.0",
    Source = new Uri("https://github.com/terraform/cosmosdb?tag=v1.1.0"),
    Notes = "",
    Inputs = new Dictionary<string, string> { { "Name", "The Name of the Cosmos Db instance" } },
    Outputs = []
});

var terraformDriver = host.Services.GetRequiredService<IResourceDriver>();

await terraformDriver.ValidateAsync(cosmosDbResourceTemplate,
    new Dictionary<string, string>() { { "Name", "TestCosmos" } });