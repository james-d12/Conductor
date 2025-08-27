using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Terraform;

public static class TerraformCommandLine
{
    public static async Task<bool> GenerateInputJsonAsync(string executeDirectory, string outputJsonPath)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "terraform",
            Arguments = $"output -json > \'{outputJsonPath}'\"",
            WorkingDirectory = executeDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = startInfo;
        process.Start();
        await process.WaitForExitAsync();
        return File.Exists(outputJsonPath);
    }

    public static async Task<bool> GenerateOutputJsonAsync(string executeDirectory, string outputJsonPath, ILogger logger)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "terraform",
            Arguments = $"output -json > \'{outputJsonPath}'\"",
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

        logger.LogDebug("Terraform Generate Output for {Source}:\n{StdOut}", outputJsonPath, stdOut);

        if (process.ExitCode != 0)
        {
            logger.LogWarning("Could not Generate Output {Source} Due to {Error}", outputJsonPath, stdErr);
        }
        
        return File.Exists(outputJsonPath);
    }
}