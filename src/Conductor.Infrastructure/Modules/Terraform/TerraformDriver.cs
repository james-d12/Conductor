using Conductor.Core.Common.Services;
using Conductor.Core.Modules.ResourceTemplate.Domain;
using Conductor.Infrastructure.Common;
using Conductor.Infrastructure.Modules.Terraform.Models;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Modules.Terraform;

public sealed class TerraformDriver : IResourceDriver
{
    public string Name => "Terraform";

    private readonly ILogger<TerraformDriver> _logger;
    private readonly ITerraformRenderer _renderer;
    private readonly ITerraformParser _parser;

    public TerraformDriver(ILogger<TerraformDriver> logger, ITerraformRenderer renderer, ITerraformParser parser)
    {
        _logger = logger;
        _renderer = renderer;
        _parser = parser;
    }

    public async Task ValidateAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        _logger.LogInformation("Validating Template: {Template} using the Terraform Driver.", template.Name);

        ResourceTemplateVersion? latestVersion = template.LatestVersion;

        if (latestVersion is null)
        {
            _logger.LogWarning("No Version could be found for {Template} found.", template.Name);
            return;
        }

        var templateDir = Path.Combine(Path.GetTempPath(), "conductor", template.Name, latestVersion.Version);
        var cloneResult =
            await GitCommandLine.CloneAsync(latestVersion.Source, templateDir, _logger, CancellationToken.None);

        if (!cloneResult)
        {
            _logger.LogWarning("Could not clone template: {Template} from {Source}", template.Name,
                latestVersion.Source.ToString());
            return;
        }

        _logger.LogInformation("Successfully cloned Repository: {Url} to {Output}", latestVersion.Source, templateDir);

        TerraformConfig? terraformConfig = await _parser.ParseTerraformModuleAsync(templateDir);

        if (terraformConfig is null)
        {
            return;
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
            foreach (var input in invalidInputs)
            {
                _logger.LogWarning("Input: {input} was not a valid input for this terraform module {Module}.", input,
                    template.Name);
            }

            return;
        }

        var requiredInputs = terraformConfig.Variables.Values.Where(v => v.Required).ToList();
        var requiredInputsNotSatisfied = requiredInputs
            .Where(variable =>
                !inputs.Any(input => input.Key.Equals(variable.Name, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        if (requiredInputsNotSatisfied.Count > 0)
        {
            foreach (var requiredInput in requiredInputsNotSatisfied)
            {
                _logger.LogWarning("Required Input: {input} was not provided for this terraform module: {Module}.",
                    requiredInput.Name,
                    template.Name);
            }

            return;
        }

        foreach (var variable in terraformConfig.Variables)
        {
            _logger.LogInformation("Terraform Variable: {Variable}", variable.Key);
        }

        var mainTf = _renderer.Render(template, inputs);
        var outputPath = Path.Combine(templateDir, "conductor_main.tf");
        await File.WriteAllTextAsync(outputPath, mainTf);
        _logger.LogInformation("Written contents to: {FilePath}", outputPath);
    }

    public Task PlanAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        // run `terraform plan`
        throw new NotImplementedException();
    }

    public Task ApplyAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        // run `terraform apply`
        throw new NotImplementedException();
    }

    public Task DestroyAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        // run `terraform destroy`
        throw new NotImplementedException();
    }
}