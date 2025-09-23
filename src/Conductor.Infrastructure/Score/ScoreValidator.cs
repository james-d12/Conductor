using Conductor.Core.Application;
using Conductor.Core.Deployment;
using Conductor.Infrastructure.CommandLine;
using Conductor.Infrastructure.Score.Models;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Score;

public interface IScoreValidator
{
    Task<ScoreValidationResult> ValidateAsync(Deployment deployment, Application application,
        CancellationToken cancellationToken = default);
}

public sealed class ScoreValidator : IScoreValidator
{
    private readonly ILogger<ScoreValidator> _logger;
    private readonly IGitCommandLine _gitCommandLine;

    public ScoreValidator(ILogger<ScoreValidator> logger, IGitCommandLine gitCommandLine)
    {
        _logger = logger;
        _gitCommandLine = gitCommandLine;
    }

    public async Task<ScoreValidationResult> ValidateAsync(Deployment deployment, Application application,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Validating Score File for Application: {Application}", application.Name);

        var commit = deployment.CommitId.Value;
        var safeDirectoryName = application.Name.Replace(" ", "_").Trim();
        var destination = Path.Combine(Path.GetTempPath(), "conductor", "score", safeDirectoryName);

        var result = await _gitCommandLine.CloneCommitAsync(application.Repository.Url, commit, destination);

        if (!result)
        {
            _logger.LogError("Could not clone repository: {RepositoryUrl} for commit: {Commit}",
                application.Repository.Url, commit);
            return ScoreValidationResult.CloneFailed();
        }

        var scoreFile = Directory
            .GetFiles(destination, "score.yaml", SearchOption.AllDirectories)
            .FirstOrDefault();

        if (string.IsNullOrEmpty(scoreFile))
        {
            return ScoreValidationResult.ScoreFileNotFound();
        }

        return ScoreValidationResult.Valid(scoreFile);
    }
}