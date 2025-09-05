using Conductor.Core.Modules.ResourceTemplate.Domain;
using Conductor.Infrastructure.Common;
using Conductor.Infrastructure.Modules.Helm.Models;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Modules.Helm;

public interface IHelmValidator
{
    Task<HelmValidationResult> ValidateAsync(ResourceTemplate template, Dictionary<string, string> inputs);
}

public sealed class HelmValidator : IHelmValidator
{
    private readonly ILogger<HelmValidator> _logger;
    private readonly IGitCommandLine _gitCommandLine;
    private readonly IHelmParser _parser;

    public HelmValidator(ILogger<HelmValidator> logger, IGitCommandLine gitCommandLine, IHelmParser parser)
    {
        _logger = logger;
        _gitCommandLine = gitCommandLine;
        _parser = parser;
    }

    public async Task<HelmValidationResult> ValidateAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        _logger.LogInformation("Validating Template: {Template} using the Helmchart Driver.", template.Name);

        if (template.Provider != ResourceTemplateProvider.Helm)
        {
            var message = $"The template: {template.Name} is configured to use {template.Provider}";
            return HelmValidationResult.WrongProvider(message);
        }

        ResourceTemplateVersion? latestVersion = template.LatestVersion;
        if (latestVersion is null)
        {
            var message = $"No Version could be found for {template.Name} found.";
            return HelmValidationResult.TemplateNotFound(message);
        }

        var templateDir = Path.Combine(Path.GetTempPath(), "conductor", "helm", template.Name, latestVersion.Version);
        var cloneResult =
            await _gitCommandLine.CloneAsync(latestVersion.Source.BaseUrl, templateDir);

        if (!string.IsNullOrEmpty(latestVersion.Source.FolderPath))
        {
            templateDir = Path.Combine(templateDir, latestVersion.Source.FolderPath);
        }

        if (!cloneResult)
        {
            var message = $"Could not clone template: {template.Name} from {latestVersion.Source}";
            return HelmValidationResult.ModuleNotFound(message);
        }

        _logger.LogInformation("Successfully cloned Repository: {Url} to {Output}", latestVersion.Source, templateDir);

        var config = await _parser.ParseHelmConfigAsync(templateDir);

        var invalidInputs = inputs
            .Where(i =>
                !config.Any(input => input.Key.Equals(i.Key, StringComparison.OrdinalIgnoreCase)))
            .Select(i => i.Key)
            .ToList();

        if (invalidInputs.Count > 0)
        {
            var message = $"These inputs were not present in the helm chart: {string.Join(",", invalidInputs)}";
            return HelmValidationResult.InputNotPresent(message);
        }

        return HelmValidationResult.Valid(config);
    }
}