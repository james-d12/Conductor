using Conductor.Domain.Models;

namespace Conductor.Domain.Services;

public interface IResourceDriver
{
    string Name { get; }

    Task ValidateAsync(ResourceTemplate template, Dictionary<string, string> inputs);
    Task PlanAsync(ResourceTemplate template, Dictionary<string, string> inputs);
    Task ApplyAsync(ResourceTemplate template, Dictionary<string, string> inputs);
    Task DestroyAsync(ResourceTemplate template, Dictionary<string, string> inputs);
}