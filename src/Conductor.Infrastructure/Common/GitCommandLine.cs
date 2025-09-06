using Conductor.Infrastructure.Common.CommandLine;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Common;

public interface IGitCommandLine
{
    Task<bool> CloneAsync(Uri source, string destination);
}

public sealed class GitCommandLine : IGitCommandLine
{
    private readonly ILogger<GitCommandLine> _logger;

    public GitCommandLine(ILogger<GitCommandLine> logger)
    {
        _logger = logger;
    }

    public async Task<bool> CloneAsync(Uri source, string destination)
    {
        var arguments =
            $"clone --depth 1 --single-branch --no-tags --no-recurse-submodules \"{source}\" \"{destination}\"";
        CommandLineResult cliResult =
            await new CommandLineBuilder("git")
                .WithArguments(arguments)
                .ExecuteAsync();

        _logger.LogDebug("Git clone output for {Source}:\n{StdOut}", source, cliResult.StdOut);

        if (cliResult.ExitCode != 0)
        {
            _logger.LogWarning("Could not Clone {Source} Due to {Error}", source, cliResult.StdErr);
        }

        return Directory.Exists(destination);
    }
}