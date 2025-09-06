using Conductor.Core.Modules.ResourceTemplate.Requests;

namespace Conductor.Core.Modules.ResourceTemplate.Domain;

public readonly record struct ResourceTemplateId(Guid Id)
{
    public ResourceTemplateId() : this(Guid.NewGuid())
    {
    }
}

public sealed record ResourceTemplate
{
    public ResourceTemplateId Id { get; private init; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ResourceTemplateProvider Provider { get; private set; }
    public DateTime CreatedAt { get; private init; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<ResourceTemplateVersion> _versions = [];
    public IReadOnlyList<ResourceTemplateVersion> Versions => _versions.AsReadOnly();

    public ResourceTemplateVersion? LatestVersion =>
        _versions.LastOrDefault(v => v.State == ResourceTemplateVersionState.Active);

    private ResourceTemplate()
    {
    }

    public static ResourceTemplate Create(CreateResourceTemplateRequest request)
    {
        ArgumentException.ThrowIfNullOrEmpty(request.Name);
        ArgumentException.ThrowIfNullOrEmpty(request.Description);

        return new ResourceTemplate
        {
            Id = new ResourceTemplateId(),
            Name = request.Name,
            Description = request.Description,
            Provider = request.Provider,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public static ResourceTemplate CreateWithVersion(CreateResourceTemplateWithVersionRequest request)
    {
        var resourceTemplate = Create(new CreateResourceTemplateRequest
        {
            Name = request.Name,
            Description = request.Description,
            Provider = request.Provider
        });
        resourceTemplate.AddVersion(new CreateNewResourceTemplateVersionRequest
        {
            Version = request.Version,
            Source = request.Source,
            Notes = request.Notes,
            State = request.State
        });
        return resourceTemplate;
    }

    public void AddVersion(CreateNewResourceTemplateVersionRequest versionRequest)
    {
        if (_versions.Any(v => v.Version == versionRequest.Version))
        {
            throw new InvalidOperationException($"Version '{versionRequest.Version}' already exists.");
        }

        if (_versions.Any(v => v.Source == versionRequest.Source))
        {
            throw new InvalidOperationException($"Source '{versionRequest.Source}' already exists.");
        }

        var newVersion = ResourceTemplateVersion.Create(new CreateResourceTemplateVersionRequest
        {
            TemplateId = Id,
            Version = versionRequest.Version,
            Source = versionRequest.Source,
            Notes = versionRequest.Notes,
            State = versionRequest.State,
            CreatedAt = DateTime.UtcNow
        });

        _versions.Add(newVersion);
        UpdatedAt = DateTime.UtcNow;
    }
}