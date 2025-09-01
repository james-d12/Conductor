using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Modules.Terraform;

public interface ITerraformCommandLine
{
    Task<bool> GenerateOutputJsonAsync(string executeDirectory, string outputJsonPath);
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

        var startInfo = new ProcessStartInfo
        {
            FileName = "terraform-config-inspect",
            Arguments = "--json .",
            WorkingDirectory = executeDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = startInfo;
        process.Start();

        var stdOutTask = process.StandardOutput.ReadToEndAsync();
        var stdErrTask = process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        var stdOut = await stdOutTask;
        var stdErr = await stdErrTask;

        _logger.LogDebug("Terraform Generate Output for {Source}:\n{StdOut}", outputJsonPath, stdOut);

        if (process.ExitCode != 0)
        {
            var errorOutput = !string.IsNullOrEmpty(stdErr) ? stdErr : stdOut;
            _logger.LogWarning("Could not Generate Output {Source} Due to {Error}", outputJsonPath, errorOutput);
            return false;
        }

        await File.WriteAllTextAsync(outputJsonPath, stdOut);
        _logger.LogInformation("Created JSON Output file from Terraform Config Inspect: {File}", outputJsonPath);
        return File.Exists(outputJsonPath);
    }
}