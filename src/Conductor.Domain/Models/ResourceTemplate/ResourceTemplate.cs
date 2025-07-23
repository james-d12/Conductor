namespace Conductor.Domain.Models.ResourceTemplate;

public readonly record struct ResourceTemplateId(Guid Id)
{
    public ResourceTemplateId() : this(Guid.NewGuid())
    {
    }
}

/// <summary>
/// Represents an external requirement that an application needs (e.g. A Cosmos Db with a Container)
/// </summary>
public sealed record ResourceTemplate
{
    public required ResourceTemplateId Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required ResourceTemplateProvider Provider { get; init; }
    public required ResourceTemplateType Type { get; init; }
    private readonly List<ResourceTemplateVersion> _versions = [];

    public void AddVersion(string version, Uri source, string notes)
    {
        if (_versions.Any(v => v.Version == version))
        {
            throw new InvalidOperationException("Version already exists.");
        }

        _versions.Insert(0, new ResourceTemplateVersion
        {
            TemplateId = Id,
            Version = version,
            Source = source,
            CreatedAt = DateTime.UtcNow,
            Notes = notes
        });
    }

    public void RemoveVersion(string version)
    {
        _versions.RemoveAll(v => v.Version == version);
    }

    public ResourceTemplateVersion? LatestVersion => _versions.FirstOrDefault();

    public ResourceTemplateVersion? GetVersion(string version) =>
        _versions.FirstOrDefault(v => v.Version == version);
}