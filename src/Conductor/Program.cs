using Conductor.Domain.Models.ResourceTemplate;
using Conductor.Domain.Models.ResourceTemplate.Requests;

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
    Version = "1.0.0",
    Source = new Uri("https://github.com/terraform/cosmosdb?tag=v1.0.0"),
    Notes = "",
    Inputs = [],
    Outputs = []
});