using AutoFixture;
using Conductor.Domain.Models.ResourceTemplate;
using Conductor.Domain.Models.ResourceTemplate.Requests;

namespace Conductor.Domain.Tests.Models.ResourceTemplate;

public sealed class ResourceTemplateTests
{
    private readonly IFixture _fixture = new Fixture();

    [Fact]
    public void Create_ShouldInitializeTemplateCorrectly()
    {
        var request = _fixture.Build<CreateResourceTemplateRequest>()
            .With(x => x.Provider, ResourceTemplateProvider.Terraform)
            .With(x => x.Type, ResourceTemplateType.AzureCosmosDb)
            .Create();

        var template = Domain.Models.ResourceTemplate.ResourceTemplate.Create(request);

        Assert.Equal(request.Name, template.Name);
        Assert.Equal(request.Description, template.Description);
        Assert.Equal(request.Provider, template.Provider);
        Assert.Equal(request.Type, template.Type);
        Assert.NotEqual(default, template.Id);
        Assert.Empty(template.Versions);
        Assert.Null(template.LatestVersion);
    }

    [Fact]
    public void CreateWithVersion_ShouldAddVersionCorrectly()
    {
        var request = _fixture.Build<CreateResourceTemplateWithVersionRequest>()
            .With(x => x.Provider, ResourceTemplateProvider.Terraform)
            .With(x => x.Type, ResourceTemplateType.AzureCosmosDb)
            .With(x => x.Source, new Uri("https://example.com/v1"))
            .Create();

        var template = Domain.Models.ResourceTemplate.ResourceTemplate.CreateWithVersion(request);

        Assert.Single(template.Versions);
        Assert.Equal(request.Version, template.LatestVersion?.Version);
    }

    [Fact]
    public void AddVersion_ShouldAddNewVersion()
    {
        var templateRequest = _fixture.Build<CreateResourceTemplateRequest>()
            .With(x => x.Provider, ResourceTemplateProvider.Terraform)
            .With(x => x.Type, ResourceTemplateType.AzureCosmosDb)
            .Create();

        var template = Domain.Models.ResourceTemplate.ResourceTemplate.Create(templateRequest);

        var versionRequest = new CreateNewResourceTemplateVersionRequest
        {
            Version = "1.0.0",
            Source = new Uri("https://example.com/v1"),
            Notes = _fixture.Create<string>(),
            Inputs = _fixture.Create<Dictionary<string, string>>(),
            Outputs = _fixture.Create<Dictionary<string, string>>()
        };

        template.AddVersion(versionRequest);

        Assert.Single(template.Versions);
        Assert.Equal("1.0.0", template.LatestVersion?.Version);
    }

    [Fact]
    public void AddVersion_ShouldThrow_WhenVersionExists()
    {
        var template = Domain.Models.ResourceTemplate.ResourceTemplate.Create(_fixture
            .Build<CreateResourceTemplateRequest>()
            .With(x => x.Provider, ResourceTemplateProvider.Terraform)
            .With(x => x.Type, ResourceTemplateType.AzureCosmosDb)
            .Create());

        var source1 = new Uri("https://example.com/v1");
        var source2 = new Uri("https://example.com/v2");

        var version = "1.0.0";

        template.AddVersion(new CreateNewResourceTemplateVersionRequest
        {
            Version = version,
            Source = source1,
            Notes = _fixture.Create<string>(),
            Inputs = _fixture.Create<Dictionary<string, string>>(),
            Outputs = _fixture.Create<Dictionary<string, string>>()
        });

        var duplicate = new CreateNewResourceTemplateVersionRequest
        {
            Version = version,
            Source = source2,
            Notes = _fixture.Create<string>(),
            Inputs = _fixture.Create<Dictionary<string, string>>(),
            Outputs = _fixture.Create<Dictionary<string, string>>()
        };

        var ex = Assert.Throws<InvalidOperationException>(() => template.AddVersion(duplicate));
        Assert.Contains("Version '1.0.0' already exists", ex.Message);
    }

    [Fact]
    public void AddVersion_ShouldThrow_WhenSourceExists()
    {
        var template = Domain.Models.ResourceTemplate.ResourceTemplate.Create(_fixture
            .Build<CreateResourceTemplateRequest>()
            .With(x => x.Provider, ResourceTemplateProvider.Terraform)
            .With(x => x.Type, ResourceTemplateType.AzureCosmosDb)
            .Create());

        var source = new Uri("https://example.com/shared");

        template.AddVersion(new CreateNewResourceTemplateVersionRequest
        {
            Version = "1.0.0",
            Source = source,
            Notes = _fixture.Create<string>(),
            Inputs = _fixture.Create<Dictionary<string, string>>(),
            Outputs = _fixture.Create<Dictionary<string, string>>()
        });

        var duplicate = new CreateNewResourceTemplateVersionRequest
        {
            Version = "2.0.0",
            Source = source,
            Notes = _fixture.Create<string>(),
            Inputs = _fixture.Create<Dictionary<string, string>>(),
            Outputs = _fixture.Create<Dictionary<string, string>>()
        };

        var ex = Assert.Throws<InvalidOperationException>(() => template.AddVersion(duplicate));
        Assert.Contains("Source 'https://example.com/shared' already exists", ex.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Create_ShouldThrow_WhenNameOrDescriptionIsInvalid(string? invalid)
    {
        var valid = _fixture.Create<string>();

        Assert.Throws<ArgumentException>(() => Domain.Models.ResourceTemplate.ResourceTemplate.Create(
            new CreateResourceTemplateRequest
            {
                Name = invalid!,
                Description = valid,
                Provider = ResourceTemplateProvider.Terraform,
                Type = ResourceTemplateType.AzureCosmosDb
            }));

        Assert.Throws<ArgumentException>(() => Domain.Models.ResourceTemplate.ResourceTemplate.Create(
            new CreateResourceTemplateRequest
            {
                Name = valid,
                Description = invalid!,
                Provider = ResourceTemplateProvider.Terraform,
                Type = ResourceTemplateType.AzureCosmosDb
            }));
    }
}