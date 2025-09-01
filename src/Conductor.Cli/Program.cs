using Conductor.Core;
using Conductor.Core.Common.Services;
using Conductor.Core.Modules.ResourceTemplate.Domain;
using Conductor.Core.Modules.ResourceTemplate.Requests;
using Conductor.Infrastructure;
using Conductor.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddCoreServices();
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Configuration.AddUserSecrets<Program>();

await builder.Services.ApplyMigrations();

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

await terraformDriver.PlanAsync(azureStorageAccount,
    new Dictionary<string, string>() { { "name", "Payments" } });

await terraformDriver.PlanAsync(azureVirtualNetwork,
    new Dictionary<string, string>()
    {
        { "name", "PaymentsNetwork" },
        { "address_space", "thing" },
        { "location", "uk south" },
        { "resource_group_name", "dev" }
    });