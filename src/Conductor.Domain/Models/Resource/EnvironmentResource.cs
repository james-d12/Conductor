namespace Conductor.Domain.Models.Resource;

public sealed record EnvironmentResource : Resource
{
    public required Environment Environment { get; init; }

    private EnvironmentResource()
    {
    }

    public static EnvironmentResource Create(
        string name,
        ResourceTemplateVersion templateVersion,
        Environment environment,
        Dictionary<string, string> inputs)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(environment);
        ArgumentNullException.ThrowIfNull(templateVersion);

        return new EnvironmentResource
        {
            Id = new ResourceId(),
            Name = name,
            TemplateVersion = templateVersion,
            Environment = environment,
            Inputs = inputs
        };
    }
}