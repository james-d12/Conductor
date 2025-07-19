/* Workflows
 * Deploying a new application and its new resources to each environment (developer)
 * Deploying a new version of the code for an application (developer)
 * Deploying an update to a resource that is used by many applications to each environment (developer / platform engineer)
 * Removing a resource that is used by many applications to each environment (developer / paltform engineer)
 * Updating resource templates (e.g. updating cosmos db module) (Platform Engineer)
 * Updating the version of Cosmos Db in DEV to V2 (Platform Engineer)
 */

using System.Globalization;
using Conductor.Domain.Models;
using Conductor.Domain.Models.Resource;
using Conductor.Domain.Services;
using Environment = Conductor.Domain.Models.Environment;

var me = new Owner
{
    Id = Guid.NewGuid(),
    Name = "James",
    EmailAddress = "jamestest@gmail.com"
};

var cosmosResourceTemplate = new ResourceTemplate
{
    Id = new ResourceTemplateId(Guid.NewGuid()),
    Name = "Cosmos Db",
    Description = "The Cosmos Db Terraform Resource Template",
    Provider = ResourceTemplate.ResourceTemplateProvider.Terraform,
    Type = ResourceTemplate.ResourceTemplateType.Database
};

var cosmosV1Template = new ResourceTemplateVersion
{
    TemplateId = cosmosResourceTemplate.Id,
    Version = "v1.0",
    Source = new Uri("https://github.com/test"),
    CreatedAt = default,
    Notes = "Version 1 of our Cosmos Db Module"
};

var devEnvironment = Environment.Create("Dev", "The Dev Environment");
devEnvironment.AddResource("Cosmos Db Dev", cosmosV1Template, []);

var app = Application.Create("Shopping Api");
app.AddResource("Blob Storage Container", cosmosV1Template, []);
app.AddOwner(me);

var deploymentManager = new DeploymentManager();
var deployment = deploymentManager.DeployAppToEnvironment(app, devEnvironment);