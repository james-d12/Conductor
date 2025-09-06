using Conductor.Infrastructure.Common;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Modules.Terraform;

public interface ITerraformCommandLine
{
    Task<bool> GenerateOutputJsonAsync(string executeDirectory, string outputJsonPath);
    Task<bool> RunInitAsync(string executeDirectory);
    Task<bool> RunValidateAsync(string executeDirectory);
    Task<bool> RunPlanAsync(string executeDirectory);
}

public sealed class TerraformCommandLine : ITerraformCommandLine
{
    private readonly ILogger<TerraformCommandLine> _logger;

    public TerraformCommandLine(ILogger<TerraformCommandLine> logger)
    {
        _logger = logger;
    }

    public async Task<bool> GenerateOutputJsonAsync(string executeDirectory, string outputJsonPath)
    {
        _logger.LogInformation("Generating Output using Terraform Config Inspect for {Directory} to {OutputPath}",
            executeDirectory, outputJsonPath);

        CommandLineResult cliResult =
            await new CommandLineBuilder("terraform-config-inspect")
                .WithArguments("--json .")
                .WithWorkingDirectory(executeDirectory)
                .ExecuteAsync();

        _logger.LogDebug("Terraform Generate Output for {Source}:\n{StdOut}", outputJsonPath, cliResult.StdOut);

        if (cliResult.ExitCode != 0)
        {
            var errorOutput = !string.IsNullOrEmpty(cliResult.StdErr) ? cliResult.StdErr : cliResult.StdOut;
            _logger.LogWarning("Could not Generate Output {Source} Due to {Error}", outputJsonPath, errorOutput);
            return false;
        }

        await File.WriteAllTextAsync(outputJsonPath, cliResult.StdOut);
        _logger.LogInformation("Created JSON Output file from Terraform Config Inspect: {File}", outputJsonPath);
        return File.Exists(outputJsonPath);
    }

    public async Task<bool> RunInitAsync(string executeDirectory)
    {
        CommandLineResult cliResult =
            await new CommandLineBuilder("terraform")
                .WithArguments("init")
                .WithWorkingDirectory(executeDirectory)
                .ExecuteAsync();

        _logger.LogDebug("Terraform Init Output for {Source}:\n{StdOut}", executeDirectory, cliResult.StdOut);

        if (cliResult.ExitCode != 0)
        {
            var errorOutput = !string.IsNullOrEmpty(cliResult.StdErr) ? cliResult.StdErr : cliResult.StdOut;
            _logger.LogWarning("Could not Init Terraform Module {Source} Due to {Error}", executeDirectory,
                errorOutput);
            return false;
        }

        return true;
    }

    public async Task<bool> RunValidateAsync(string executeDirectory)
    {
        CommandLineResult cliResult =
            await new CommandLineBuilder("terraform")
                .WithArguments("validate")
                .WithWorkingDirectory(executeDirectory)
                .ExecuteAsync();

        _logger.LogDebug("Terraform Validate Output for {Source}:\n{StdOut}", executeDirectory, cliResult.StdOut);

        if (cliResult.ExitCode != 0)
        {
            var errorOutput = !string.IsNullOrEmpty(cliResult.StdErr) ? cliResult.StdErr : cliResult.StdOut;
            _logger.LogWarning("Could not Validate Terraform Module {Source} Due to {Error}", executeDirectory,
                errorOutput);
            return false;
        }

        return true;
    }

    public async Task<bool> RunPlanAsync(string executeDirectory)
    {
        _logger.LogInformation("Running Terraform plan in {Directory}", executeDirectory);
        CommandLineResult cliResult =
            await new CommandLineBuilder("terraform")
                .WithArguments("plan -input=false -out=plan.tfplan")
                .WithWorkingDirectory(executeDirectory)
                .ExecuteAsync();

        _logger.LogDebug("Terraform Plan Output for {Source}:\n{StdOut}", executeDirectory, cliResult.StdOut);

        if (cliResult.ExitCode != 0)
        {
            var errorOutput = !string.IsNullOrEmpty(cliResult.StdErr) ? cliResult.StdErr : cliResult.StdOut;
            _logger.LogWarning("Could not Plan Terraform Module {Source} Due to {Error}", executeDirectory,
                errorOutput);
            return false;
        }

        return true;
    }
}