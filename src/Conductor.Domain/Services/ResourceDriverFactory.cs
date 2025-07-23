using Conductor.Domain.Models.Resource;
using Conductor.Domain.Models.ResourceTemplate;

namespace Conductor.Domain.Services;

public interface IResourceDriverFactory
{
    IResourceDriver GetDriver(ResourceTemplateProvider provider);
}

public sealed class ResourceDriverFactory : IResourceDriverFactory
{
    private readonly Dictionary<ResourceTemplateProvider, IResourceDriver> _drivers;

    public ResourceDriverFactory(IEnumerable<IResourceDriver> drivers)
    {
        _drivers = drivers.ToDictionary(d => Enum.Parse<ResourceTemplateProvider>(d.Name, ignoreCase: true));
    }

    public IResourceDriver GetDriver(ResourceTemplateProvider provider)
    {
        return _drivers.TryGetValue(provider, out var driver)
            ? driver
            : throw new InvalidOperationException($"No driver found for provider: {provider}");
    }
}