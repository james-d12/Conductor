namespace Conductor.Core.ResourceTemplate;

public readonly record struct ResourceTemplateId(Guid Value)
{
    public ResourceTemplateId() : this(Guid.NewGuid())
    {
    }
}

public sealed record ResourceTemplate
{
    public ResourceTemplateId Id { get; private init; }
    public string Name { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ResourceTemplateProvider Provider { get; private set; }
    public DateTime CreatedAt { get; private init; }
    public DateTime UpdatedAt { get; private set; }
    private readonly List<ResourceTemplateVersion> _versions = [];
    public IReadOnlyList<ResourceTemplateVersion> Versions => _versions.AsReadOnly();


    private ResourceTemplate()
    {
    }

    public static ResourceTemplate Create(string name, string type, string description,
        ResourceTemplateProvider provider)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(type);
        ArgumentException.ThrowIfNullOrEmpty(description);

        return new ResourceTemplate
        {
            Id = new ResourceTemplateId(),
            Name = name,
            Type = type,
            Description = description,
            Provider = provider,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public ResourceTemplateVersion? GetLatestVersion()
    {
        return _versions.LastOrDefault(r => r.State == ResourceTemplateVersionState.Active);
    }

    public void AddVersion(string version, ResourceTemplateVersionSource source, string notes,
        ResourceTemplateVersionState state)
    {
        if (_versions.Any(v => v.Version == version))
        {
            throw new InvalidOperationException($"Version '{version}' already exists.");
        }

        if (_versions.Any(v => v.Source == source))
        {
            throw new InvalidOperationException($"Source '{source}' already exists.");
        }

        var newVersion = ResourceTemplateVersion.Create(
            templateId: Id,
            version: version,
            source: source,
            notes: notes,
            state: state,
            createdAt: DateTime.UtcNow);

        _versions.Add(newVersion);
        UpdatedAt = DateTime.UtcNow;
    }
}