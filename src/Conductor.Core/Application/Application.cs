namespace Conductor.Core.Application;

public readonly record struct ApplicationId(Guid Value)
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
    public required Repository Repository { get; init; }
    public required List<Deployment.Deployment> Deployments { get; init; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }

    private Application()
    {
    }

    public static Application Create(string name, Repository repository)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return new Application
        {
            Id = new ApplicationId(),
            Name = name,
            Repository = repository,
            Deployments = [],
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }

    public void Deploy(Deployment.Deployment deployment)
    {
        Deployments.Add(deployment);
        UpdatedAt = DateTime.Now;
    }

    public void Undeploy(Deployment.Deployment deployment)
    {
        Deployments.Remove(deployment);
        UpdatedAt = DateTime.Now;
    }
}