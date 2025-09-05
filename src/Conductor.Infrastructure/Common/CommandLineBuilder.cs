using System.Diagnostics;

namespace Conductor.Infrastructure.Common;

public sealed class CommandLineBuilder
{
    private readonly ProcessStartInfo _startInfo;

    public CommandLineBuilder(string fileName)
    {
        _startInfo = new ProcessStartInfo
        {
            FileName = fileName,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
    }

    public CommandLineBuilder WithArguments(string arguments)
    {
        _startInfo.Arguments = arguments;
        return this;
    }

    public CommandLineBuilder WithWorkingDirectory(string workingDirectory)
    {
        _startInfo.WorkingDirectory = workingDirectory;
        return this;
    }

    public async Task<CommandLineResult> ExecuteAsync()
    {
        using var process = new Process();
        process.StartInfo = _startInfo;
        process.Start();

        var stdOutTask = process.StandardOutput.ReadToEndAsync();
        var stdErrTask = process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        var stdOut = await stdOutTask;
        var stdErr = await stdErrTask;

        return new CommandLineResult(stdOut, stdErr, process.ExitCode);
    }
}