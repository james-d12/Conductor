using Conductor.Core.Modules.ResourceTemplate.Domain;

namespace Conductor.Core.Common.Services;

public interface IResourceDriver
{
    string Name { get; }
    Task PlanAsync(ResourceTemplate template, Dictionary<string, string> inputs);
    Task ApplyAsync(ResourceTemplate template, Dictionary<string, string> inputs);
    Task DestroyAsync(ResourceTemplate template, Dictionary<string, string> inputs);
}