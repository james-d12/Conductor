using Conductor.Core;
using Conductor.Core.Application;
using Conductor.Core.Deployment;
using Conductor.Core.Provisioning;
using Conductor.Core.ResourceTemplate;
//using Conductor.Core.ResourceTemplate.Requests;
using Conductor.Infrastructure;
using Conductor.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Environment = Conductor.Core.Environment.Environment;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddCoreServices();
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Configuration.AddUserSecrets<Program>();

await builder.Services.ApplyMigrations();

using var host = builder.Build();

var azureStorageAccount = ResourceTemplate.Create(
    name: "Azure Storage Account",
    type: "azure.storage-account",
    description: "Azure Storage Account Terraform Module",
    provider: ResourceTemplateProvider.Terraform
);

azureStorageAccount.AddVersion(
    version: "1.0.0",
    source: new ResourceTemplateVersionSource
    {
        BaseUrl = new Uri("https://github.com/aztfm/terraform-azurerm-storage-account.git"),
        FolderPath = string.Empty,
        Tag = string.Empty
    },
    notes: string.Empty,
    state: ResourceTemplateVersionState.Active
);

var azureVirtualNetwork = ResourceTemplate.Create(
    name: "Azure Virtual Network",
    type: "azure.virtual-network",
    description: "Azure Virtual Network Terraform Module",
    provider: ResourceTemplateProvider.Terraform
);

azureVirtualNetwork.AddVersion(
    version: "1.0.0",
    source: new ResourceTemplateVersionSource
    {
        BaseUrl = new Uri("https://github.com/aztfm/terraform-azurerm-virtual-network.git"),
        FolderPath = string.Empty,
        Tag = string.Empty
    },
    notes: string.Empty,
    state: ResourceTemplateVersionState.Active
);

var azureContainerRegistry = ResourceTemplate.Create(
    name: "Azure Container Registry",
    type: "azure.container-registry",
    description: "Azure Container Registry Terraform Module",
    provider: ResourceTemplateProvider.Terraform
);

azureContainerRegistry.AddVersion(
    version: "1.0.0",
    source: new ResourceTemplateVersionSource
    {
        BaseUrl = new Uri("https://github.com/Azure/terraform-azurerm-avm-res-containerregistry-registry.git"),
        FolderPath = string.Empty,
        Tag = string.Empty
    },
    notes: string.Empty,
    state: ResourceTemplateVersionState.Active
);

var argoCdTemplate = ResourceTemplate.Create(
    name: "ArgoCD Helm Chart",
    type: "helm.argocd",
    description: "An ArgoCD Helm Chart",
    provider: ResourceTemplateProvider.Helm
);

argoCdTemplate.AddVersion(
    version: "1.0",
    source: new ResourceTemplateVersionSource
    {
        BaseUrl = new Uri("https://github.com/bitnami/charts.git"),
        FolderPath = "bitnami/argo-cd",
        Tag = string.Empty
    },
    notes: string.Empty,
    state: ResourceTemplateVersionState.Active
);


var exampleApp = Application.Create("example-app", new Repository
{
    Name = "example repository",
    Url = new Uri("https://github.com/james-d12/Conductor-Example.git"),
    Provider = RepositoryProvider.GitHub
});

var devEnvironment = Environment.Create("dev", "The Development Environment");

var commit = new Commit
{
    Id = new CommitId("7b926d5c23d0e806c62d4c86e25fc73564efb8a1"),
    Message = "Updated Application"
};

var deployment = Deployment.Create(exampleApp.Id, devEnvironment.Id, commit.Id);

var resourceTemplateRepository = host.Services.GetRequiredService<IResourceTemplateRepository>();
await resourceTemplateRepository.CreateAsync(azureStorageAccount);
await resourceTemplateRepository.CreateAsync(azureVirtualNetwork);
await resourceTemplateRepository.CreateAsync(azureContainerRegistry);
await resourceTemplateRepository.CreateAsync(argoCdTemplate);

var provisioningService = host.Services.GetRequiredService<ProvisioningService>();

await provisioningService.ProvisionAsync(exampleApp, deployment, CancellationToken.None);