using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Modules.Terraform;

public static class TerraformCommandLine
{
    public static async Task<bool> GenerateOutputJsonAsync(string executeDirectory, string outputJsonPath,
        ILogger logger)
    {
        logger.LogInformation("Generating Output using Terraform Config Inspect for {Directory} to {OutputPath}",
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

        logger.LogInformation("Terraform Generate Output for {Source}:\n{StdOut}", outputJsonPath, stdOut);

        if (process.ExitCode == 0)
        {
            await File.WriteAllTextAsync(outputJsonPath, stdOut);
            logger.LogDebug("Created JSON Output file from Terraform Config Inspect: {File}", outputJsonPath);
            return File.Exists(outputJsonPath);
        }
        else
        {
            var errorOutput = !string.IsNullOrEmpty(stdErr) ? stdErr : stdOut;
            logger.LogWarning("Could not Generate Output {Source} Due to {Error}", outputJsonPath, errorOutput);
            return false;
        }
    }
}