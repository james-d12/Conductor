using Conductor.Domain.Models.Resource;
using Conductor.Domain.Models.ResourceTemplate;

namespace Conductor.Domain.Services;

public interface IResourceDriver
{
    string Name { get; }

    Task ValidateAsync(ResourceTemplateVersion version, Dictionary<string, string> inputs);
    Task PlanAsync(ResourceTemplateVersion version, Dictionary<string, string> inputs);
    Task ApplyAsync(ResourceTemplateVersion version, Dictionary<string, string> inputs);
    Task DestroyAsync(ResourceTemplateVersion version, Dictionary<string, string> inputs);
}