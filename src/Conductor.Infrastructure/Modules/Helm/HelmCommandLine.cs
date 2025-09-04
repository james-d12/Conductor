using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Modules.Helm;

public interface IHelmCommandLine
{
    Task<bool> GenerateOutputJsonAsync(string executeDirectory, string outputJsonPath);
}

public sealed class HelmCommandLine : IHelmCommandLine
{
    private readonly ILogger<HelmCommandLine> _logger;

    public HelmCommandLine(ILogger<HelmCommandLine> logger)
    {
        _logger = logger;
    }

    public async Task<bool> GenerateOutputJsonAsync(string executeDirectory, string outputJsonPath)
    {
        _logger.LogInformation("Generating Output using Helm CLI for {Directory} to {OutputPath}",
            executeDirectory, outputJsonPath);

        var startInfo = new ProcessStartInfo
        {
            FileName = "helm",
            Arguments = "show values .",
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

        _logger.LogDebug("Helm Generate Output for {Source}:\n{StdOut}", outputJsonPath, stdOut);

        if (process.ExitCode != 0)
        {
            var errorOutput = !string.IsNullOrEmpty(stdErr) ? stdErr : stdOut;
            _logger.LogWarning("Could not Generate Output {Source} Due to {Error}", outputJsonPath, errorOutput);
            return false;
        }

        await File.WriteAllTextAsync(outputJsonPath, stdOut);
        _logger.LogInformation("Created JSON Output file from Helm: {File}", outputJsonPath);
        return File.Exists(outputJsonPath);
    }
}