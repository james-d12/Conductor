using Conductor.Core.Modules.ResourceTemplate;
using Conductor.Core.Modules.ResourceTemplate.Domain;
using Conductor.Infrastructure.Modules.Score;
using Conductor.Infrastructure.Modules.Score.Models;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Services;

public sealed class ResourceProvisioner
{
    private readonly ILogger<ResourceProvisioner> _logger;
    private readonly IResourceTemplateRepository _resourceTemplateRepository;
    private readonly IResourceFactory _resourceFactory;
    private readonly IScoreParser _scoreParser;

    public ResourceProvisioner(ILogger<ResourceProvisioner> logger, IScoreParser scoreParser,
        IResourceTemplateRepository resourceTemplateRepository, IResourceFactory resourceFactory)
    {
        _logger = logger;
        _scoreParser = scoreParser;
        _resourceTemplateRepository = resourceTemplateRepository;
        _resourceFactory = resourceFactory;
    }

    public async Task StartAsync(string fileName, bool delete = false)
    {
        ScoreFile? scoreFile = await _scoreParser.ParseAsync(fileName);

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

                ResourceTemplate? resourceTemplate = await _resourceTemplateRepository.GetByTypeAsync(type);

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

            if (delete)
            {
                await _resourceFactory.DeleteAsync(provisionInputs, directoryName);
            }
            else
            {
                await _resourceFactory.ProvisionAsync(provisionInputs, directoryName);
            }
        }
    }
}