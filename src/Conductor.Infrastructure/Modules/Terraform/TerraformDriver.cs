using Conductor.Core.Modules.ResourceTemplate.Domain;
using Conductor.Infrastructure.Modules.Terraform.Models;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Modules.Terraform;

public interface ITerraformDriver
{
    Task PlanAsync(ResourceTemplate template, Dictionary<string, string> inputs);
    Task ApplyAsync(ResourceTemplate template, Dictionary<string, string> inputs);
    Task DestroyAsync(ResourceTemplate template, Dictionary<string, string> inputs);
}

public sealed class TerraformDriver : ITerraformDriver
{
    private readonly ILogger<TerraformDriver> _logger;
    private readonly ITerraformValidator _validator;
    private readonly ITerraformCommandLine _commandLine;
    private readonly ITerraformState _state;

    public TerraformDriver(ILogger<TerraformDriver> logger, ITerraformValidator validator,
        ITerraformCommandLine commandLine, ITerraformState state)
    {
        _logger = logger;
        _validator = validator;
        _commandLine = commandLine;
        _state = state;
    }

    public async Task PlanAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        var result = await _validator.ValidateAsync(template, inputs);

        if (result.State != TerraformValidationResultState.Valid)
        {
            _logger.LogError("Terraform Validation for {Template} Failed due to: {State} with Message: {Message}",
                template.Name, result.State,
                result.Message);
            return;
        }

        _logger.LogInformation("Terraform Validation for {Template} Passed.", template.Name);

        var stateDirectory = await _state.SetupDirectoryAsync(template, result, inputs);

        var initResult = await _commandLine.RunInitAsync(stateDirectory);

        if (initResult.ExitCode != 0)
        {
            return;
        }

        var validateResult = await _commandLine.RunValidateAsync(stateDirectory);

        if (validateResult.ExitCode != 0)
        {
            return;
        }

        var planResult = await _commandLine.RunPlanAsync(stateDirectory);

        if (planResult.ExitCode != 0)
        {
            return;
        }

        _logger.LogInformation("Successfully run plan for {Template}", template.Name);
    }

    public async Task ApplyAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        var result = await _validator.ValidateAsync(template, inputs);
    }

    public Task DestroyAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        throw new NotImplementedException();
    }
}