namespace Conductor.Engine.Domain.Application;

/// <summary>
/// Represents an Application that encompasses the Git Repository,
/// Pipelines, required Resources, and deployed environments for an Application  
/// </summary>
public sealed record Application
{
    public required ApplicationId Id { get; init; }
    public required string Name { get; init; }
    public required Repository Repository { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }

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
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }

    public static Application Create(CreateApplicationRequest request)
    {
        ArgumentException.ThrowIfNullOrEmpty(request.Name);

        return new Application
        {
            Id = new ApplicationId(),
            Name = request.Name,
            Repository = new Repository
            {
                Name = request.Repository.Name,
                Url = request.Repository.Url,
                Provider = request.Repository.Provider
            },
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }
}