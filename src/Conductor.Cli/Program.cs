using Conductor.Core;
using Conductor.Core.Modules.ResourceTemplate;
using Conductor.Core.Modules.ResourceTemplate.Domain;
using Conductor.Core.Modules.ResourceTemplate.Requests;
using Conductor.Infrastructure;
using Conductor.Infrastructure.Modules.Score;
using Conductor.Infrastructure.Services;
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
    Type = "azure.storage-account",
    Description = "Azure Storage Account Terraform Module",
    Provider = ResourceTemplateProvider.Terraform,
    Version = "1.0.0",
    Source = new ResourceTemplateVersionSource
    {
        BaseUrl = new Uri("https://github.com/aztfm/terraform-azurerm-storage-account.git"),
        FolderPath = string.Empty,
        Tag = string.Empty
    },
    Notes = string.Empty,
    State = ResourceTemplateVersionState.Active
});

var azureVirtualNetwork = ResourceTemplate.CreateWithVersion(new CreateResourceTemplateWithVersionRequest
{
    Name = "Azure Virtual Network",
    Type = "azure.virtual-network",
    Description = "Azure Virtual Network Terraform Module",
    Provider = ResourceTemplateProvider.Terraform,
    Version = "1.0.0",
    Source = new ResourceTemplateVersionSource
    {
        BaseUrl = new Uri("https://github.com/aztfm/terraform-azurerm-virtual-network.git"),
        FolderPath = string.Empty,
        Tag = string.Empty
    },
    Notes = string.Empty,
    State = ResourceTemplateVersionState.Active
});

var azureContainerRegistry = ResourceTemplate.CreateWithVersion(new CreateResourceTemplateWithVersionRequest
{
    Name = "Azure Container Registry",
    Type = "azure.container-registry",
    Description = "Azure Container Registry Terraform Module",
    Provider = ResourceTemplateProvider.Terraform,
    Version = "1.0.0",
    Source = new ResourceTemplateVersionSource
    {
        BaseUrl = new Uri("https://github.com/Azure/terraform-azurerm-avm-res-containerregistry-registry.git"),
        FolderPath = string.Empty,
        Tag = string.Empty
    },
    Notes = string.Empty,
    State = ResourceTemplateVersionState.Active
});

var argoCdTemplate = ResourceTemplate.CreateWithVersion(new CreateResourceTemplateWithVersionRequest
{
    Name = "ArgoCD Helm Chart",
    Type = "helm.argocd",
    Description = "An ArgoCD Helm Chart",
    Provider = ResourceTemplateProvider.Helm,
    Version = "1.0",
    Source = new ResourceTemplateVersionSource
    {
        BaseUrl = new Uri("https://github.com/bitnami/charts.git"),
        FolderPath = "bitnami/argo-cd",
        Tag = string.Empty
    },
    Notes = string.Empty,
    State = ResourceTemplateVersionState.Active
});

var resourceTemplateRepository = host.Services.GetRequiredService<IResourceTemplateRepository>();
var scoreParser = host.Services.GetRequiredService<IScoreParser>();

var resourceProvisioner = host.Services.GetRequiredService<IResourceProvisioner>();

await resourceTemplateRepository.CreateAsync(azureStorageAccount);
await resourceTemplateRepository.CreateAsync(azureVirtualNetwork);
await resourceTemplateRepository.CreateAsync(argoCdTemplate);

var scoreFile = await scoreParser.ParseAsync("./example.yaml");

if (scoreFile?.Resources != null)
{
    Console.WriteLine("Provisioning Resources for score file");
    foreach (var resource in scoreFile.Resources)
    {
        var type = resource.Value.Type.Trim().ToLower();
        var inputs = resource.Value.Parameters;

        ResourceTemplate? resourceTemplate = await resourceTemplateRepository.GetByTypeAsync(type);

        if (resourceTemplate is null)
        {
            Console.WriteLine("Could not get template");
            continue;
        }

        if (inputs is null)
        {
            Console.WriteLine("inputs is null bruv");
            continue;
        }

        await resourceProvisioner.ProvisionAsync(resourceTemplate, inputs);
    }
}