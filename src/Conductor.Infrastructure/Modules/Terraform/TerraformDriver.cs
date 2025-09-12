using Conductor.Core.Modules.ResourceTemplate.Domain;
using Conductor.Infrastructure.Common.CommandLine;
using Conductor.Infrastructure.Modules.Terraform.Models;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Modules.Terraform;

public interface ITerraformDriver
{
    Task<TerraformPlanResult> PlanAsync(ResourceTemplate template, Dictionary<string, string> inputs);
    Task<TerraformPlanDestroyResult> PlanDestroyAsync(ResourceTemplate template, Dictionary<string, string> inputs);
    Task ApplyAsync(TerraformPlanResult planResult);
    Task DestroyAsync(TerraformPlanDestroyResult planDestroyResult);
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
            return new TerraformPlanResult(TerraformPlanResultState.PreValidationFailed, validationResult.Message);
        }

        _logger.LogInformation("Terraform Validation for {Template} Passed.", template.Name);

        var stateDirectory = await _state.SetupDirectoryAsync(template, validationResult, inputs);

        var initResult = await _commandLine.RunInitAsync(stateDirectory);

        if (initResult.ExitCode != 0)
        {
            return new TerraformPlanResult(stateDirectory, TerraformPlanResultState.InitFailed);
        }

        _logger.LogDebug("Terraform Init Output: {Output}", initResult.StdOut);

        var validateResult = await _commandLine.RunValidateAsync(stateDirectory);

        if (validateResult.ExitCode != 0)
        {
            _logger.LogWarning("Terraform Validate Failed: {ExitCode} with {Output}", validateResult.ExitCode,
                validateResult.StdErr);
            return new TerraformPlanResult(stateDirectory, TerraformPlanResultState.ValidateFailed);
        }

        _logger.LogDebug("Terraform Validate Output: {Output}", validateResult.StdOut);

        var planResult = await _commandLine.RunPlanAsync(stateDirectory);

        if (planResult.ExitCode != 0)
        {
            return new TerraformPlanResult(stateDirectory, TerraformPlanResultState.PlanFailed, planResult);
        }

        _logger.LogDebug("Terraform Plan Output: {Output}", planResult.StdOut);

        _logger.LogInformation("Successfully run plan for {Template}", template.Name);

        return new TerraformPlanResult(stateDirectory, TerraformPlanResultState.Success, planResult);
    }

    public async Task<TerraformPlanDestroyResult> PlanDestroyAsync(ResourceTemplate template,
        Dictionary<string, string> inputs)
    {
        var validationResult = await _validator.ValidateAsync(template, inputs);

        if (validationResult.State != TerraformValidationResultState.Valid)
        {
            _logger.LogError("Terraform Validation for {Template} Failed due to: {State} with Message: {Message}",
                template.Name, validationResult.State,
                validationResult.Message);
            return new TerraformPlanDestroyResult();
        }

        _logger.LogInformation("Terraform Validation for {Template} Passed.", template.Name);

        var stateDirectory = await _state.SetupDirectoryAsync(template, validationResult, inputs);

        var initResult = await _commandLine.RunInitAsync(stateDirectory);

        if (initResult.ExitCode != 0)
        {
            return new TerraformPlanDestroyResult(stateDirectory);
        }

        _logger.LogDebug("Terraform Init Output: {Output}", initResult.StdOut);

        var validateResult = await _commandLine.RunValidateAsync(stateDirectory);

        if (validateResult.ExitCode != 0)
        {
            _logger.LogWarning("Terraform Validate Failed: {ExitCode} with {Output}", validateResult.ExitCode,
                validateResult.StdErr);
            return new TerraformPlanDestroyResult(stateDirectory);
        }

        _logger.LogDebug("Terraform Validate Output: {Output}", validateResult.StdOut);

        var planDestroyResult = await _commandLine.RunPlanDestroyAsync(stateDirectory);

        if (planDestroyResult.ExitCode != 0)
        {
            return new TerraformPlanDestroyResult(stateDirectory, planDestroyResult);
        }

        _logger.LogDebug("Terraform Destroy Plan Output: {Output}", planDestroyResult.StdOut);

        _logger.LogInformation("Successfully run destroy plan for {Template}", template.Name);

        return new TerraformPlanDestroyResult(stateDirectory, planDestroyResult);
    }

    public async Task ApplyAsync(TerraformPlanResult planResult)
    {
        switch (planResult.State)
        {
            case TerraformPlanResultState.PreValidationFailed:
            case TerraformPlanResultState.InitFailed:
            case TerraformPlanResultState.ValidateFailed:
            case TerraformPlanResultState.PlanFailed:
                _logger.LogWarning("Plan Was not in a valid state: {Message} {State}",
                    planResult.Message, planResult.State.ToString());
                break;
            case TerraformPlanResultState.Success:
                _logger.LogInformation("Running Terraform Apply in {Directory}", planResult.StateDirectory);
                CommandLineResult applyResult = await _commandLine.RunApplyAsync(planResult.StateDirectory);
                _logger.LogInformation("Terraform Apply Result: {Result}", applyResult.StdOut);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async Task DestroyAsync(TerraformPlanDestroyResult planDestroyResult)
    {
        if (planDestroyResult.PlanDestroyCommandLineResult?.ExitCode != 0)
        {
            _logger.LogWarning("Plan Result did not have a successful exit code: {ExitCode}",
                planDestroyResult.PlanDestroyCommandLineResult?.ExitCode);
            return;
        }

        if (string.IsNullOrEmpty(planDestroyResult.StateDirectory))
        {
            _logger.LogWarning("Plan Result did not have valid state directory: {StateDirectory}",
                planDestroyResult.StateDirectory);
            return;
        }

        var destroyResult = await _commandLine.RunDestroyAsync(planDestroyResult.StateDirectory);

        _logger.LogInformation("Terraform Apply Result: {Result}", destroyResult.StdOut);
    }
}