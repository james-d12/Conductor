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
    Source = new ResourceTemplateVersionSource
    {
        BaseUrl = new Uri("https://github.com/aztfm/terraform-azurerm-storage-account.git"),
        FolderPath = string.Empty
    },
    Notes = "",
    State = ResourceTemplateVersionState.Active
});

var azureVirtualNetwork = ResourceTemplate.CreateWithVersion(new CreateResourceTemplateWithVersionRequest
{
    Name = "Azure Virtual Network",
    Description = "Azure Virtual Network Terraform Module",
    Provider = ResourceTemplateProvider.Terraform,
    Type = ResourceTemplateType.AzureStorageAccount,
    Version = "1.0.0",
    Source = new ResourceTemplateVersionSource
    {
        BaseUrl = new Uri("https://github.com/aztfm/terraform-azurerm-virtual-network.git"),
        FolderPath = string.Empty
    },
    Notes = "",
    State = ResourceTemplateVersionState.Active
});

var argoCdTemplate = ResourceTemplate.CreateWithVersion(new CreateResourceTemplateWithVersionRequest
{
    Name = "ArgoCD Helm Chart",
    Description = "An ArgoCD Helm Chart",
    Provider = ResourceTemplateProvider.Helm,
    Type = ResourceTemplateType.HelmChart,
    Version = "1.0",
    Source = new ResourceTemplateVersionSource
    {
        BaseUrl = new Uri("https://github.com/bitnami/charts.git"),
        FolderPath = "bitnami/argo-cd"
    },
    Notes = string.Empty,
    State = ResourceTemplateVersionState.Active
});

var resourceDriverFactory = host.Services.GetRequiredService<IResourceDriverFactory>();

var terraformDriver = resourceDriverFactory.GetDriver(azureStorageAccount.Provider);
var helmDriver = resourceDriverFactory.GetDriver(argoCdTemplate.Provider);

await terraformDriver.PlanAsync(azureStorageAccount,
    new Dictionary<string, string>()
    {
        { "name", "payments" },
        { "account_replication_type", "LRS" },
        { "account_tier", "Standard" },
        { "location", "uksouth" },
        { "resource_group_name", "dev-rg" }
    });

/*
await terraformDriver.PlanAsync(azureVirtualNetwork,
    new Dictionary<string, string>()
    {
        { "name", "PaymentsNetwork" },
        { "address_space", "['10.0.0.0/16', '10.0.0.0/16']"},
        { "location", "uk south" },
        { "resource_group_name", "dev" }
    }); */