using Conductor.Core.Application;
using Conductor.Core.Deployment;
using Conductor.Core.Provisioning.Requirements;
using Conductor.Infrastructure.Score.Models;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Score;

public sealed class ScoreRequirementDriver : IRequirementDriver
{
    private readonly ILogger<ScoreRequirementDriver> _logger;
    private readonly IScoreParser _parser;
    private readonly IScoreValidator _validator;

    public ScoreRequirementDriver(IScoreParser parser, IScoreValidator validator,
        ILogger<ScoreRequirementDriver> logger)
    {
        _parser = parser;
        _validator = validator;
        _logger = logger;
    }

    public async Task<RequirementResult> GetRequirementsAsync(Application application, Deployment deployment,
        CancellationToken cancellationToken = default)
    {
        var scoreValidationResult =
            await _validator.ValidateAsync(deployment, application, cancellationToken: cancellationToken);

        if (scoreValidationResult.State != ScoreValidationResultState.Valid)
        {
            _logger.LogError("Score Validation for {Application} failed due to: {State}",
                application.Name, scoreValidationResult.State);
            return new RequirementResult
            {
                Requirement = null
            };
        }

        ScoreFile? scoreFile = await _parser.ParseAsync(scoreValidationResult.ScoreFilePath);

        if (scoreFile is null)
        {
            _logger.LogWarning("Unable to find / parse the provided score file.");
            return new RequirementResult
            {
                Requirement = null
            };
        }

        var resources = scoreFile.Resources?.Select(r => new RequirementResource
        {
            Id = r.Key,
            Class = r.Value.Class ?? string.Empty,
            Type = r.Value.Type,
            Parameters = r.Value.Parameters
        }).ToList() ?? [];

        var requirement = new Requirement
        {
            Name = scoreFile.Metadata.Name,
            Resources = resources
        };
        return new RequirementResult() { Requirement = requirement };
    }
}