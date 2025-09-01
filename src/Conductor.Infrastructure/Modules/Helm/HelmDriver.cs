using Conductor.Core.Common.Services;
using Conductor.Core.Modules.ResourceTemplate.Domain;

namespace Conductor.Infrastructure.Modules.Helm;

public sealed class HelmDriver : IResourceDriver
{
    public string Name => "Helm";

    public Task PlanAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        throw new NotImplementedException();
    }

    public Task ApplyAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        throw new NotImplementedException();
    }
}