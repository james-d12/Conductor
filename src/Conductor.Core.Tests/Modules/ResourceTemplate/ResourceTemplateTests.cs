using AutoFixture;
using Conductor.Core.Modules.ResourceTemplate.Domain;
using Conductor.Core.Modules.ResourceTemplate.Requests;

namespace Conductor.Core.Tests.Modules.ResourceTemplate;

public sealed class ResourceTemplateTests
{
    private readonly IFixture _fixture = new Fixture();

    [Fact]
    public void Create_ShouldInitializeTemplateCorrectly()
    {
        var request = _fixture.Build<CreateResourceTemplateRequest>()
            .With(x => x.Provider, ResourceTemplateProvider.Terraform)
            .Create();

        var template = Core.Modules.ResourceTemplate.Domain.ResourceTemplate.Create(request);

        Assert.Equal(request.Name, template.Name);
        Assert.Equal(request.Description, template.Description);
        Assert.Equal(request.Provider, template.Provider);
        Assert.NotEqual(default, template.Id);
        Assert.Empty(template.Versions);
        Assert.Null(template.LatestVersion);
    }

    [Fact]
    public void CreateWithVersion_ShouldAddVersionCorrectly()
    {
        var resourceTemplateVersionSource = _fixture.Build<ResourceTemplateVersionSource>()
            .With(s => s.BaseUrl, new Uri("https://example.com/v1"))
            .Create();

        var request = _fixture.Build<CreateResourceTemplateWithVersionRequest>()
            .With(x => x.Provider, ResourceTemplateProvider.Terraform)
            .With(x => x.Source, resourceTemplateVersionSource)
            .Create();

        var template = Core.Modules.ResourceTemplate.Domain.ResourceTemplate.CreateWithVersion(request);

        Assert.Single(template.Versions);
        Assert.Equal(request.Version, template.LatestVersion?.Version);
    }

    [Fact]
    public void AddVersion_ShouldAddNewVersion()
    {
        var templateRequest = _fixture.Build<CreateResourceTemplateRequest>()
            .With(x => x.Provider, ResourceTemplateProvider.Terraform)
            .Create();

        var template = Core.Modules.ResourceTemplate.Domain.ResourceTemplate.Create(templateRequest);

        var versionRequest = new CreateNewResourceTemplateVersionRequest
        {
            Version = "1.0.0",
            Source = new ResourceTemplateVersionSource
            {
                BaseUrl = new Uri("https://example.com/v1"),
                FolderPath = string.Empty
            },
            Notes = _fixture.Create<string>(),
            State = ResourceTemplateVersionState.Active
        };

        template.AddVersion(versionRequest);

        Assert.Single(template.Versions);
        Assert.Equal("1.0.0", template.LatestVersion?.Version);
    }

    [Fact]
    public void AddVersion_ShouldThrow_WhenVersionExists()
    {
        var template = Core.Modules.ResourceTemplate.Domain.ResourceTemplate.Create(_fixture
            .Build<CreateResourceTemplateRequest>()
            .With(x => x.Provider, ResourceTemplateProvider.Terraform)
            .Create());

        var resourceTemplateVersionSource1 = _fixture.Build<ResourceTemplateVersionSource>()
            .With(s => s.BaseUrl, new Uri("https://example.com/v1"))
            .Create();

        var resourceTemplateVersionSource2 = _fixture.Build<ResourceTemplateVersionSource>()
            .With(s => s.BaseUrl, new Uri("https://example.com/v2"))
            .Create();

        var version = "1.0.0";

        template.AddVersion(new CreateNewResourceTemplateVersionRequest
        {
            Version = version,
            Source = resourceTemplateVersionSource1,
            Notes = _fixture.Create<string>(),
            State = ResourceTemplateVersionState.Active
        });

        var duplicate = new CreateNewResourceTemplateVersionRequest
        {
            Version = version,
            Source = resourceTemplateVersionSource2,
            Notes = _fixture.Create<string>(),
            State = ResourceTemplateVersionState.Active
        };

        var ex = Assert.Throws<InvalidOperationException>(() => template.AddVersion(duplicate));
        Assert.Contains("Version '1.0.0' already exists", ex.Message);
    }

    [Fact]
    public void AddVersion_ShouldThrow_WhenSourceExists()
    {
        var template = Core.Modules.ResourceTemplate.Domain.ResourceTemplate.Create(_fixture
            .Build<CreateResourceTemplateRequest>()
            .With(x => x.Provider, ResourceTemplateProvider.Terraform)
            .Create());

        var resourceTemplateVersionSource = _fixture.Build<ResourceTemplateVersionSource>()
            .With(s => s.BaseUrl, new Uri("https://example.com/shared"))
            .Create();

        template.AddVersion(new CreateNewResourceTemplateVersionRequest
        {
            Version = "1.0.0",
            Source = resourceTemplateVersionSource,
            Notes = _fixture.Create<string>(),
            State = ResourceTemplateVersionState.Active
        });

        var duplicate = new CreateNewResourceTemplateVersionRequest
        {
            Version = "2.0.0",
            Source = resourceTemplateVersionSource,
            Notes = _fixture.Create<string>(),
            State = ResourceTemplateVersionState.Active
        };

        Assert.Throws<InvalidOperationException>(() => template.AddVersion(duplicate));
    }

    [Fact]
    public void Create_ShouldThrow_WhenNameOrDescriptionIsNull()
    {
        var valid = _fixture.Create<string>();

        Assert.Throws<ArgumentNullException>(() => Core.Modules.ResourceTemplate.Domain.ResourceTemplate.Create(
            new CreateResourceTemplateRequest
            {
                Name = null!,
                Description = valid,
                Provider = ResourceTemplateProvider.Terraform,
            }));

        Assert.Throws<ArgumentNullException>(() => Core.Modules.ResourceTemplate.Domain.ResourceTemplate.Create(
            new CreateResourceTemplateRequest
            {
                Name = null!,
                Description = null!,
                Provider = ResourceTemplateProvider.Terraform,
            }));
    }

    [Fact]
    public void Create_ShouldThrow_WhenNameOrDescriptionIsEmptyString()
    {
        var valid = _fixture.Create<string>();

        Assert.Throws<ArgumentException>(() => Core.Modules.ResourceTemplate.Domain.ResourceTemplate.Create(
            new CreateResourceTemplateRequest
            {
                Name = string.Empty,
                Description = valid,
                Provider = ResourceTemplateProvider.Terraform
            }));

        Assert.Throws<ArgumentException>(() => Core.Modules.ResourceTemplate.Domain.ResourceTemplate.Create(
            new CreateResourceTemplateRequest
            {
                Name = string.Empty,
                Description = string.Empty,
                Provider = ResourceTemplateProvider.Terraform
            }));
    }
}