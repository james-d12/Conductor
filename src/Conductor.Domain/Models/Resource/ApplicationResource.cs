namespace Conductor.Domain.Models.Resource;

public sealed record ApplicationResource : Resource
{
    public required Application Application { get; init; }

    private ApplicationResource()
    {
    }

    public static ApplicationResource Create(
        string name,
        ResourceTemplateVersion templateVersion,
        Application application,
        Dictionary<string, string> inputs)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(application);
        ArgumentNullException.ThrowIfNull(templateVersion);

        return new ApplicationResource
        {
            Id = new ResourceId(),
            Name = name,
            TemplateVersion = templateVersion,
            Application = application,
            Inputs = inputs
        };
    }
}