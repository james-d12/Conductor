namespace Conductor.Core.Environment;

public readonly record struct EnvironmentId(Guid Value)
{
    public EnvironmentId() : this(Guid.NewGuid())
    {
    }
}

/// <summary>
/// Represents a deployment environment. E.g. a Kubernetes Cluster, an ArgoCD Environment, etc.
/// E.g DEV, UAT, PROD
/// </summary>
public sealed record Environment
{
    public required EnvironmentId Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required List<Deployment.Deployment> Deployments { get; init; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }

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
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }

    public void Deploy(Deployment.Deployment deployment)
    {
        Deployments.Add(deployment);
        CreatedAt = DateTime.Now;
    }

    public void Undeploy(Deployment.Deployment deployment)
    {
        Deployments.Remove(deployment);
        CreatedAt = DateTime.Now;
    }
}