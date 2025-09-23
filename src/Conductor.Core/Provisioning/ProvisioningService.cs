using Conductor.Core.Provisioning.Requirements;
using Conductor.Core.ResourceTemplate;
using Microsoft.Extensions.Logging;

namespace Conductor.Core.Provisioning;

public sealed class ProvisioningService
{
    private readonly ILogger<ProvisioningService> _logger;
    private readonly IResourceTemplateRepository _resourceTemplateRepository;
    private readonly IProvisionFactory _provisionFactory;
    private readonly IRequirementDriver _requirementDriver;

    public ProvisioningService(ILogger<ProvisioningService> logger,
        IRequirementDriver requirementDriver,
        IResourceTemplateRepository resourceTemplateRepository,
        IProvisionFactory provisionFactory)
    {
        _logger = logger;
        _requirementDriver = requirementDriver;
        _resourceTemplateRepository = resourceTemplateRepository;
        _provisionFactory = provisionFactory;
    }

    public async Task ProvisionAsync(Application.Application application,
        Deployment.Deployment deployment,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Provisioning Resources for score file");

        var requirementResult =
            await _requirementDriver.GetRequirementsAsync(application, deployment, cancellationToken);

        if (requirementResult.Requirement is null)
        {
            _logger.LogWarning("No requirement found for {Application}.", application);
            return;
        }

        var directoryName = requirementResult.Requirement.Name;
        var provisionInputs = new List<ProvisionInput>();

        foreach (var resource in requirementResult.Requirement.Resources ?? [])
        {
            var type = resource.Type.Trim().ToLower();
            var inputs = resource.Parameters;

            ResourceTemplate.ResourceTemplate? resourceTemplate =
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

            provisionInputs.Add(new ProvisionInput(resourceTemplate, inputs, resource.Id));
        }

        await _provisionFactory.ProvisionAsync(provisionInputs, directoryName);
    }
}