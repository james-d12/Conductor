using Conductor.Domain.Models.Resource;

namespace Conductor.Domain.Models;

public readonly record struct EnvironmentId(Guid Id)
{
    public EnvironmentId() : this(Guid.NewGuid())
    {
    }
}

/// <summary>
/// Represents a deployment environment. E.g. a Kubernetes Cluster, an ArgoCD Environment, etc.
/// E.g DEV, UAT, PROD
/// </summary>
public record Environment
{
    public required EnvironmentId Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required List<Deployment> Deployments { get; init; }
    public required List<EnvironmentResource> Resources { get; init; }

    private Environment()
    {
    }

    public static Environment Create(string name, string description)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(description);

        return new Environment
        {
            Id = new EnvironmentId(),
            Name = name,
            Description = description,
            Deployments = [],
            Resources = []
        };
    }

    public void AddResource(string name, ResourceTemplateVersion templateVersion, Dictionary<string, string> inputs)
    {
        var environmentResourceInstance = EnvironmentResource.Create(name, templateVersion, this, inputs);
        Resources.Add(environmentResourceInstance);
    }
}