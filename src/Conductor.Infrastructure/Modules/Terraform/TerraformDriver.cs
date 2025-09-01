using Conductor.Core.Common.Services;
using Conductor.Core.Modules.ResourceTemplate.Domain;
using Conductor.Infrastructure.Modules.Terraform.Models;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Modules.Terraform;

public sealed class TerraformDriver : IResourceDriver
{
    public string Name => "Terraform";

    private readonly ILogger<TerraformDriver> _logger;
    private readonly ITerraformValidator _validator;

    public TerraformDriver(ILogger<TerraformDriver> logger, ITerraformValidator validator)
    {
        _logger = logger;
        _validator = validator;
    }

    public async Task PlanAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        var result = await _validator.ValidateAsync(template, inputs);

        switch (result.State)
        {
            case TerraformValidationResultState.TemplateNotFound:
            case TerraformValidationResultState.ModuleNotFound:
            case TerraformValidationResultState.ModuleNotParsable:
            case TerraformValidationResultState.InputNotPresent:
            case TerraformValidationResultState.RequiredInputNotProvided:
                _logger.LogError("Terraform Validation for {Template} Failed due to: {State} with Message: {Message}",
                    template.Name, result.State,
                    result.Message);
                break;
            case TerraformValidationResultState.Valid:
                break;
        }
    }

    public async Task ApplyAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        var result = await _validator.ValidateAsync(template, inputs);
    }
}