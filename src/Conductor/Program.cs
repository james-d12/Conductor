/* Workflows
 * Deploying a new application and its new resources to each environment (developer)
 * Deploying a new version of the code for an application (developer)
 * Deploying an update to a resource that is used by many applications to each environment (developer / platform engineer)
 * Removing a resource that is used by many applications to each environment (developer / paltform engineer)
 * Updating resource templates (e.g. updating cosmos db module) (Platform Engineer)
 * Updating the version of Cosmos Db in DEV to V2 (Platform Engineer)
 */

using Conductor.Domain.Models.Application;
using Conductor.Domain.Models.Environment;
using Conductor.Domain.Models.ResourceTemplate;
using Conductor.Domain.Services;
using Environment = Conductor.Domain.Models.Environment.Environment;

var cosmosResourceTemplate = new ResourceTemplate
{
    Id = new ResourceTemplateId(),
    Name = "Cosmos Db",
    Description = "The Cosmos Db Terraform Resource Template",
    Provider = ResourceTemplateProvider.Terraform,
    Type = ResourceTemplateType.AzureCosmosDb
};
cosmosResourceTemplate.AddVersion("v1.0", new Uri("https://github.com/test"), "Version 1 of our Cosmos Db Module");

var blobStorageTemplate = new ResourceTemplate
{
    Id = new ResourceTemplateId(),
    Name = "Cosmos Db",
    Description = "The Azure Blob Storage Terraform Resource Template",
    Provider = ResourceTemplateProvider.Terraform,
    Type = ResourceTemplateType.AzureBlobStorage
};
blobStorageTemplate.AddVersion("v1.0", new Uri("https://github.com/test"), "Version 1 of our Blob Storage Module");

var cosmosDbV1 = cosmosResourceTemplate.LatestVersion;
var blobStorageTemplateV1 = blobStorageTemplate.LatestVersion;

if (cosmosDbV1 == null || blobStorageTemplateV1 == null)
{
    return;
}

// Create Dev Environment and Add Cosmos Db Dev Environment Resources
var devEnvironment = Environment.Create("Dev", "The Dev Environment");
var devCosmosDbResource = EnvironmentResource.Create("Cosmos Db Dev", cosmosDbV1, devEnvironment.Id, []);
devEnvironment.AddResource(devCosmosDbResource);

var app = Application.Create("Shopping Api");
var shoppingApiBlobStorage = ApplicationResource.Create("Blob Storage Shopping Api", blobStorageTemplateV1, app.Id, []);
app.AddResource(shoppingApiBlobStorage);

var deploymentManager = new DeploymentManager();
var deployment = deploymentManager.DeployAppToEnvironment(app, devEnvironment);