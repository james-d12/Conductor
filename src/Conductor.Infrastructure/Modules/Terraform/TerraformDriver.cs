using Conductor.Core.Modules.ResourceTemplate.Domain;
using Conductor.Infrastructure.Modules.Terraform.Models;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Modules.Terraform;

public interface ITerraformDriver
{
    Task<TerraformPlanResult> PlanAsync(ResourceTemplate template, Dictionary<string, string> inputs);
    Task ApplyAsync(TerraformPlanResult planResult);
    Task DestroyAsync(TerraformPlanResult planResult);
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

    public async Task<TerraformPlanResult> PlanAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        var validationResult = await _validator.ValidateAsync(template, inputs);

        if (validationResult.State != TerraformValidationResultState.Valid)
        {
            _logger.LogError("Terraform Validation for {Template} Failed due to: {State} with Message: {Message}",
                template.Name, validationResult.State,
                validationResult.Message);
            return new TerraformPlanResult();
        }

        _logger.LogInformation("Terraform Validation for {Template} Passed.", template.Name);

        var stateDirectory = await _state.SetupDirectoryAsync(template, validationResult, inputs);

        var initResult = await _commandLine.RunInitAsync(stateDirectory);

        if (initResult.ExitCode != 0)
        {
            return new TerraformPlanResult(stateDirectory);
        }

        var validateResult = await _commandLine.RunValidateAsync(stateDirectory);

        if (validateResult.ExitCode != 0)
        {
            return new TerraformPlanResult(stateDirectory);
        }

        var planResult = await _commandLine.RunPlanAsync(stateDirectory);

        if (planResult.ExitCode != 0)
        {
            return new TerraformPlanResult(stateDirectory, planResult);
        }

        _logger.LogInformation("Successfully run plan for {Template}", template.Name);

        return new TerraformPlanResult(stateDirectory, planResult);
    }

    public async Task ApplyAsync(TerraformPlanResult planResult)
    {
        if (planResult.PlanCommandLineResult?.ExitCode != 0 || string.IsNullOrEmpty(planResult.StateDirectory))
        {
            _logger.LogWarning("Plan Result is not in a suitable state to be applied upon.");
            return;
        }

        var applyResult = await _commandLine.RunApplyAsync(planResult.StateDirectory);
    }

    public async Task DestroyAsync(TerraformPlanResult planResult)
    {
        if (planResult.PlanCommandLineResult?.ExitCode != 0 || string.IsNullOrEmpty(planResult.StateDirectory))
        {
            _logger.LogWarning("Plan Result is not in a suitable state to be applied upon.");
            return;
        }

        var deleteResult = await _commandLine.RunDeleteAsync(planResult.StateDirectory);
    }
}