using Conductor.Core.Modules.ResourceTemplate.Domain;
using Conductor.Infrastructure.Common;
using Conductor.Infrastructure.Modules.Terraform.Models;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Modules.Terraform;

public interface ITerraformValidator
{
    Task<TerraformValidationResult> ValidateAsync(ResourceTemplate template,
        Dictionary<string, string> inputs);
}

public sealed class TerraformValidator : ITerraformValidator
{
    private readonly ILogger<TerraformValidator> _logger;
    private readonly IGitCommandLine _gitCommandLine;
    private readonly ITerraformRenderer _renderer;
    private readonly ITerraformParser _parser;

    public TerraformValidator(ILogger<TerraformValidator> logger, ITerraformRenderer renderer, ITerraformParser parser,
        IGitCommandLine gitCommandLine)
    {
        _logger = logger;
        _renderer = renderer;
        _parser = parser;
        _gitCommandLine = gitCommandLine;
    }

    public async Task<TerraformValidationResult> ValidateAsync(ResourceTemplate template,
        Dictionary<string, string> inputs)
    {
        _logger.LogInformation("Validating Template: {Template} using the Terraform Driver.", template.Name);

        if (template.Provider != ResourceTemplateProvider.Terraform)
        {
            var message = $"The template: {template.Name} is configured to use {template.Provider}";
            return TerraformValidationResult.WrongProvider(message);
        }

        ResourceTemplateVersion? latestVersion = template.LatestVersion;
        if (latestVersion is null)
        {
            var message = $"No Version could be found for {template.Name} found.";
            return TerraformValidationResult.TemplateNotFound(message);
        }

        var templateDir = Path.Combine(Path.GetTempPath(), "conductor", "terraform", template.Name,
            latestVersion.Version);
        var cloneResult =
            await _gitCommandLine.CloneAsync(latestVersion.Source.BaseUrl, templateDir, CancellationToken.None);

        if (!string.IsNullOrEmpty(latestVersion.Source.FolderPath))
        {
            templateDir = Path.Combine(templateDir, latestVersion.Source.FolderPath);
        }

        if (!cloneResult)
        {
            var message = $"Could not clone template: {template.Name} from {latestVersion.Source}";
            return TerraformValidationResult.ModuleNotFound(message);
        }

        _logger.LogInformation("Successfully cloned Repository: {Url} to {Output}", latestVersion.Source, templateDir);

        TerraformConfig? terraformConfig = await _parser.ParseTerraformModuleAsync(templateDir);

        if (terraformConfig is null)
        {
            var message = $"Could not parse module: {template.Name} from {latestVersion.Source}";
            return TerraformValidationResult.ModuleNotParsable(message);
        }

        var invalidInputs = inputs
            .Where(i =>
                !terraformConfig.Variables.Keys.Any(key => key.Equals(i.Key, StringComparison.OrdinalIgnoreCase)))
            .Select(i => i.Key)
            .ToList();

        _logger.LogInformation("Terraform variable keys: {keys}",
            string.Join(",", terraformConfig.Variables.Keys.ToList()));

        if (invalidInputs.Count > 0)
        {
            var message = $"These inputs were not present in the terraform module: {string.Join(",", invalidInputs)}";
            return TerraformValidationResult.InputNotPresent(message);
        }

        var requiredInputs = terraformConfig.Variables.Values.Where(v => v.Required).ToList();
        var requiredInputsNotSatisfied = requiredInputs
            .Where(variable =>
                !inputs.Any(input => input.Key.Equals(variable.Name, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        if (requiredInputsNotSatisfied.Count > 0)
        {
            var requiredInputNames = string.Join(",", requiredInputsNotSatisfied
                .Select(r => $"{r.Name}:{r.Type}"));
            var message =
                $"These inputs are required in the terraform module, but were not provided: {requiredInputNames}";
            return TerraformValidationResult.RequiredInputNotProvided(message);
        }

        var mainTf = _renderer.Render(template, inputs);
        var outputPath = Path.Combine(templateDir, "conductor_main.tf");
        await File.WriteAllTextAsync(outputPath, mainTf);
        _logger.LogInformation("Written contents to: {FilePath}", outputPath);

        return TerraformValidationResult.Valid(terraformConfig);
    }
}