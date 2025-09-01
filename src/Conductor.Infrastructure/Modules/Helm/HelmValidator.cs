using Conductor.Core.Modules.ResourceTemplate.Domain;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Modules.Helm;

public interface IHelmchartValidator
{
    Task ValidateAsync(ResourceTemplate template, Dictionary<string, string> inputs);
}

public sealed class HelmValidator : IHelmchartValidator
{
    private readonly ILogger<HelmValidator> _logger;

    public HelmValidator(ILogger<HelmValidator> logger)
    {
        _logger = logger;
    }

    public Task ValidateAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        _logger.LogInformation("Validating Template: {Template} using the Helmchart Driver.", template.Name);



        return Task.CompletedTask;
    }
}