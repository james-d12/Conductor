using Conductor.Domain.Models.Resource;

namespace Conductor.Domain.Models;

public readonly record struct ApplicationId(Guid Id)
{
    public ApplicationId() : this(Guid.NewGuid())
    {
    }
}

/// <summary>
/// Represents an Application that encompasses the Git Repository,
/// Pipelines, required Resources, and deployed environments for an Application  
/// </summary>
public sealed record Application
{
    public required ApplicationId Id { get; init; }
    public required string Name { get; init; }
    public required List<Deployment> Deployments { get; init; }
    public required List<ApplicationResource> Resources { get; init; }
    public required List<Owner> Owners { get; init; }

    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }

    private Application()
    {
    }

    public static Application Create(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return new Application
        {
            Id = new ApplicationId(),
            Name = name,
            Deployments = [],
            Resources = [],
            Owners = [],
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }

    public void AddResource(string name, ResourceTemplateVersion templateVersion, Dictionary<string, string> inputs)
    {
        var resourceInstance = ApplicationResource.Create(name, templateVersion, this, inputs);
        Resources.Add(resourceInstance);
        UpdatedAt = DateTime.Now;
    }

    public void AddOwner(Owner owner)
    {
        Owners.Add(owner);
        UpdatedAt = DateTime.Now;
    }
}