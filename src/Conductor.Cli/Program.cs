using Conductor.Core;
using Conductor.Core.Common.Services;
using Conductor.Core.Modules.ResourceTemplate.Domain;
using Conductor.Core.Modules.ResourceTemplate.Requests;
using Conductor.Infrastructure;
using Conductor.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddCoreServices();
builder.Services.AddPersistenceServices();

await builder.Services.ApplyMigrations();

builder.Configuration.AddUserSecrets<Program>();

builder.Logging.ClearProviders();
builder.Logging.AddJsonConsole();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddLogging();

using var host = builder.Build();

var azureStorageAccount = ResourceTemplate.CreateWithVersion(new CreateResourceTemplateWithVersionRequest
{
    Name = "Azure Storage Account",
    Description = "Azure Storage Account Terraform Module",
    Provider = ResourceTemplateProvider.Terraform,
    Type = ResourceTemplateType.AzureStorageAccount,
    Version = "1.0.0",
    Source = new Uri("https://github.com/aztfm/terraform-azurerm-storage-account.git"),
    Notes = ""
});

var azureVirtualNetwork = ResourceTemplate.CreateWithVersion(new CreateResourceTemplateWithVersionRequest
{
    Name = "Azure Virtual Network",
    Description = "Azure Virtual Network Terraform Module",
    Provider = ResourceTemplateProvider.Terraform,
    Type = ResourceTemplateType.AzureStorageAccount,
    Version = "1.0.0",
    Source = new Uri("https://github.com/aztfm/terraform-azurerm-virtual-network.git"),
    Notes = ""
});

var terraformDriver = host.Services.GetRequiredService<IResourceDriver>();

await terraformDriver.ValidateAsync(azureStorageAccount,
    new Dictionary<string, string>() { { "name", "Payments" } });

await terraformDriver.ValidateAsync(azureVirtualNetwork,
    new Dictionary<string, string>() { { "Name", "PaymentsNetwork" } });