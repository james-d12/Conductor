using Conductor.Core.Common.Services;
using Conductor.Core.Modules.ResourceTemplate.Domain;
using Conductor.Infrastructure.Modules.Terraform.Models;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Modules.Terraform;

public sealed class TerraformDriver : IResourceDriver
{
    public string Name => "Terraform";

    private readonly ILogger<TerraformDriver> _logger;
    private readonly ITerraformValidator _validator;
    private readonly ITerraformCommandLine _commandLine;
    private readonly ITerraformRenderer _renderer;

    public TerraformDriver(ILogger<TerraformDriver> logger, ITerraformValidator validator,
        ITerraformCommandLine commandLine, ITerraformRenderer renderer)
    {
        _logger = logger;
        _validator = validator;
        _commandLine = commandLine;
        _renderer = renderer;
    }

    public async Task PlanAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        var result = await _validator.ValidateAsync(template, inputs);

        switch (result.State)
        {
            case TerraformValidationResultState.TemplateNotFound:
            case TerraformValidationResultState.ModuleNotFound:
            case TerraformValidationResultState.ModuleNotParsable:
            case TerraformValidationResultState.InputNotPresent:
            case TerraformValidationResultState.RequiredInputNotProvided:
                _logger.LogError("Terraform Validation for {Template} Failed due to: {State} with Message: {Message}",
                    template.Name, result.State,
                    result.Message);
                break;
            case TerraformValidationResultState.Valid:
                _logger.LogInformation("Terraform Validation for {Template} Passed.", template.Name);


                var stateDirectory = Path.Combine(Path.GetTempPath(), "conductor", "terraform", "state",
                    template.Name.Replace(" ", "."));

                Directory.CreateDirectory(stateDirectory);

                // Create main.tf
                var mainTf = _renderer.RenderMainTf(template, result.ModuleDirectory, inputs);
                _logger.LogInformation("Render output: {Output}", mainTf);
                var mainTfOutputPath = Path.Combine(stateDirectory, "main.tf");
                await File.WriteAllTextAsync(mainTfOutputPath, mainTf);
                _logger.LogInformation("Created main.tf to: {FilePath}", mainTfOutputPath);

                // Create providers.tf
                var providersTf =
                    _renderer.RenderProvidersTf([new TerraformProvider("azurerm", "hashicorp/azurerm", ">= 4.43.0")]);
                _logger.LogInformation("Render output: {Output}", mainTf);
                var providersTfOutputPath = Path.Combine(stateDirectory, "providers.tf");
                await File.WriteAllTextAsync(providersTfOutputPath, providersTf);
                _logger.LogInformation("Created providers.tf to: {FilePath}", providersTfOutputPath);

                var initResult = await _commandLine.RunInitAsync(stateDirectory);

                if (!initResult)
                {
                    return;
                }

                var validateResult = await _commandLine.RunValidateAsync(stateDirectory);

                if (!validateResult)
                {
                    return;
                }

                var planResult = await _commandLine.RunPlanAsync(stateDirectory);

                break;
        }
    }

    public async Task ApplyAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        var result = await _validator.ValidateAsync(template, inputs);
    }
}