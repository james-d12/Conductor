using Conductor.Infrastructure.CommandLine;
using Conductor.Infrastructure.Terraform.Models;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Terraform;

public interface ITerraformDriver
{
    Task<TerraformPlanResult> PlanAsync(List<TerraformPlanInput> terraformPlanInputs, string folderName,
        bool destroy = false);

    Task ApplyAsync(TerraformPlanResult planResult);
    Task DestroyAsync(TerraformPlanResult planResult);
}

public sealed class TerraformDriver : ITerraformDriver
{
    private readonly ILogger<TerraformDriver> _logger;
    private readonly ITerraformValidator _validator;
    private readonly ITerraformCommandLine _commandLine;
    private readonly ITerraformProjectBuilder _projectBuilder;

    public TerraformDriver(ILogger<TerraformDriver> logger, ITerraformValidator validator,
        ITerraformCommandLine commandLine, ITerraformProjectBuilder projectBuilder)
    {
        _logger = logger;
        _validator = validator;
        _commandLine = commandLine;
        _projectBuilder = projectBuilder;
    }

    public async Task<TerraformPlanResult> PlanAsync(List<TerraformPlanInput> terraformPlanInputs, string folderName,
        bool destroy = false)
    {
        var validationResults = await _validator.ValidateAsync(terraformPlanInputs);

        var validResults = new Dictionary<TerraformPlanInput, TerraformValidationResult.ValidResult>();

        foreach (var result in validationResults)
        {
            switch (result.Value)
            {
                case TerraformValidationResult.ValidResult vr:
                    validResults.Add(result.Key, vr);
                    break;
                default:
                    _logger.LogError("Validation failed for {Template}: {State} - {Message}",
                        result.Key.Template.Name, result.Value.State, result.Value.Message);
                    break;
            }
        }

        if (validResults.Count == 0)
        {
            _logger.LogError(
                "Could not perform Terraform Plan, as not all provided inputs were validated successfully");
            return new TerraformPlanResult(TerraformPlanResultState.PreValidationFailed,
                "Could not validate all inputs.");
        }

        var stateDirectory = await _projectBuilder.BuildProject(validResults, folderName);

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

        CommandLineResult planResult = destroy
            ? await _commandLine.RunPlanDestroyAsync(stateDirectory)
            : await _commandLine.RunPlanAsync(stateDirectory);

        if (planResult.ExitCode != 0)
        {
            return new TerraformPlanResult(stateDirectory, TerraformPlanResultState.PlanFailed, planResult);
        }

        _logger.LogDebug("Terraform Plan Output: {Output}", planResult.StdOut);

        _logger.LogInformation("Successfully run plan for {Folder}", folderName);

        return new TerraformPlanResult(stateDirectory, TerraformPlanResultState.Success, planResult);
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

    public async Task DestroyAsync(TerraformPlanResult planResult)
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
                _logger.LogInformation("Running Terraform Destroy in {Directory}", planResult.StateDirectory);
                CommandLineResult applyResult = await _commandLine.RunDestroyAsync(planResult.StateDirectory);
                _logger.LogInformation("Terraform Destroy Result: {Result}", applyResult.StdOut);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}