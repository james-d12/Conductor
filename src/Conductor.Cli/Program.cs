using Conductor.Core;
using Conductor.Core.Modules.Application.Domain;
using Conductor.Core.Modules.Deployment.Domain;
using Conductor.Core.Modules.ResourceTemplate;
using Conductor.Core.Modules.ResourceTemplate.Domain;
using Conductor.Core.Modules.ResourceTemplate.Requests;
using Conductor.Infrastructure;
using Conductor.Infrastructure.Services;
using Conductor.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Environment = Conductor.Core.Modules.Environment.Domain.Environment;

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

var paymentApi = Application.Create("payment-api", new Repository
{
    Id = Guid.NewGuid(),
    Name = "payment api repository",
    Url = new Uri("https://github.com/james-d12/Panda.git"),
    Provider = RepositoryProvider.GitHub
});

var devEnvironment = Environment.Create("dev", "The Development Environment");

var commit = new Commit
{
    Id = new CommitId("dsoaid9asid9"),
    Message = "Updated Application"
};

var deployment = Deployment.Create(paymentApi.Id, devEnvironment.Id, commit.Id);

var resourceTemplateRepository = host.Services.GetRequiredService<IResourceTemplateRepository>();
await resourceTemplateRepository.CreateAsync(azureStorageAccount);
await resourceTemplateRepository.CreateAsync(azureVirtualNetwork);
await resourceTemplateRepository.CreateAsync(azureContainerRegistry);
await resourceTemplateRepository.CreateAsync(argoCdTemplate);

var resourceProvisioner = host.Services.GetRequiredService<ResourceProvisioner>();

await resourceProvisioner.StartAsync("example.yaml");
await resourceProvisioner.StartAsync("example-after.yaml");