using Conductor.Core.Application.Domain;
using Conductor.Core.Deployment.Domain;
using Conductor.Core.ResourceTemplate;
using Conductor.Core.ResourceTemplate.Domain;
using Conductor.Infrastructure.Score;
using Conductor.Infrastructure.Score.Models;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Resources;

public interface IResourceProvisioner
{
    Task StartAsync(Application application, Deployment deployment, CancellationToken cancellationToken);
}

public sealed class ResourceProvisioner : IResourceProvisioner
{
    private readonly ILogger<ResourceProvisioner> _logger;
    private readonly IResourceTemplateRepository _resourceTemplateRepository;
    private readonly IResourceFactory _resourceFactory;
    private readonly IScoreParser _scoreParser;
    private readonly IScoreValidator _scoreValidator;

    public ResourceProvisioner(ILogger<ResourceProvisioner> logger, IScoreParser scoreParser,
        IResourceTemplateRepository resourceTemplateRepository, IResourceFactory resourceFactory,
        IScoreValidator scoreValidator)
    {
        _logger = logger;
        _scoreParser = scoreParser;
        _resourceTemplateRepository = resourceTemplateRepository;
        _resourceFactory = resourceFactory;
        _scoreValidator = scoreValidator;
    }

    public async Task StartAsync(Application application, Deployment deployment, CancellationToken cancellationToken)
    {
        var scoreValidationResult =
            await _scoreValidator.ValidateAsync(deployment, application, cancellationToken: cancellationToken);

        if (scoreValidationResult.State != ScoreValidationResultState.Valid)
        {
            _logger.LogError("Score Validation for {Application} failed due to: {State}",
                application.Name, scoreValidationResult.State);
            return;
        }

        ScoreFile? scoreFile = await _scoreParser.ParseAsync(scoreValidationResult.ScoreFilePath);

        if (scoreFile is null)
        {
            _logger.LogWarning("Unable to find / parse the provided score file.");
            return;
        }

        if (scoreFile.Resources is not null)
        {
            _logger.LogInformation("Provisioning Resources for score file");

            var directoryName = scoreFile.Metadata.Name;

            var provisionInputs = new List<ProvisionInput>();

            foreach (var resource in scoreFile.Resources ?? [])
            {
                var type = resource.Value.Type.Trim().ToLower();
                var inputs = resource.Value.Parameters;

                ResourceTemplate? resourceTemplate =
                    await _resourceTemplateRepository.GetByTypeAsync(type, cancellationToken);

                if (resourceTemplate is null)
                {
                    _logger.LogInformation("Could not get resource template for: {Type}", type);
                    continue;
                }

                if (inputs is null)
                {
                    _logger.LogInformation("No inputs present in the score file");
                    continue;
                }

                provisionInputs.Add(new ProvisionInput(resourceTemplate, inputs, resource.Key));
            }

            await _resourceFactory.ProvisionAsync(provisionInputs, directoryName);
        }
    }
}