/* Workflows
 * Deploying a new application and its new resources to each environment (developer)
 * Deploying a new version of the code for an application (developer)
 * Deploying an update to a resource that is used by many applications to each environment (developer / platform engineer)
 * Removing a resource that is used by many applications to each environment (developer / paltform engineer)
 * Updating resource templates (e.g. updating cosmos db module) (Platform Engineer)
 * Updating the version of Cosmos Db in DEV to V2 (Platform Engineer)
 */

using Conductor.Domain.Models;
using Conductor.Domain.Models.Resource;
using Conductor.Domain.Services;
using Environment = Conductor.Domain.Models.Environment;

var cosmosResourceTemplate = new ResourceTemplate
{
    Id = new ResourceTemplateId(),
    Name = "Cosmos Db",
    Description = "The Cosmos Db Terraform Resource Template",
    Provider = ResourceTemplateProvider.Terraform,
    Type = ResourceTemplateType.Database
};

var blobStorageTemplate = new ResourceTemplate
{
    Id = new ResourceTemplateId(),
    Name = "Cosmos Db",
    Description = "The Azure Blob Storage Terraform Resource Template",
    Provider = ResourceTemplateProvider.Terraform,
    Type = ResourceTemplateType.Storage
};

var cosmosV1Template = new ResourceTemplateVersion
{
    TemplateId = cosmosResourceTemplate.Id,
    Version = "v1.0",
    Source = new Uri("https://github.com/test"),
    CreatedAt = default,
    Notes = "Version 1 of our Cosmos Db Module"
};

var blobStorageTemplateV1 = new ResourceTemplateVersion
{
    TemplateId = blobStorageTemplate.Id,
    Version = "v1.0",
    Source = new Uri("https://github.com/test"),
    CreatedAt = default,
    Notes = "Version 1 of our Blob Storage Module"
};

// Create Dev Environment and Add Cosmos Db Dev Environment Resources
var devEnvironment = Environment.Create("Dev", "The Dev Environment");
var devCosmosDbResource = EnvironmentResource.Create("Cosmos Db Dev", cosmosV1Template, devEnvironment, []);
devEnvironment.AddResource(devCosmosDbResource);

var app = Application.Create("Shopping Api");
var shoppingApiBlobStorage = ApplicationResource.Create("Blob Storage Shopping Api", blobStorageTemplateV1, app, []);
app.AddResource(shoppingApiBlobStorage);

var deploymentManager = new DeploymentManager();
var deployment = deploymentManager.DeployAppToEnvironment(app, devEnvironment);