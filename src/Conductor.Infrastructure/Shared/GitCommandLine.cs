using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Shared;

public sealed class GitCommandLine
{
    public static async Task<bool> CloneAsync(Uri source, string destination, ILogger logger, CancellationToken cancellationToken)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments =
                $"clone --depth 1 --single-branch --no-tags --no-recurse-submodules \"{source.ToString()}\" \"{destination}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = startInfo;
        process.Start();

        var stdOutTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
        var stdErrTask = process.StandardError.ReadToEndAsync(cancellationToken);

        await process.WaitForExitAsync(cancellationToken);

        var stdOut = await stdOutTask;
        var stdErr = await stdErrTask;

        logger.LogDebug("Git clone output for {Source}:\n{StdOut}", source, stdOut);

        if (process.ExitCode != 0)
        {
            logger.LogWarning("Could not Clone {Source} Due to {Error}", source, stdErr);
        }

        return Directory.Exists(destination);
    }
}