using Conductor.Core.Modules.ResourceTemplate.Domain;
using Conductor.Infrastructure.Modules.Helm;
using Conductor.Infrastructure.Modules.Terraform;
using Conductor.Infrastructure.Modules.Terraform.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Conductor.Infrastructure.Services;

public interface IResourceFactory
{
    Task ProvisionAsync(List<ProvisionInput> provisionInputs, string folderName);
    Task DeleteAsync(ResourceTemplate template, Dictionary<string, string> inputs);
}

public sealed record ProvisionInput(ResourceTemplate Template, Dictionary<string, string> Inputs, string Key);

public sealed class ResourceFactory : IResourceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ResourceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ProvisionAsync(List<ProvisionInput> provisionInputs, string folderName)
    {
        var terraformProvisionInputs = provisionInputs
            .Where(p => p.Template.Provider == ResourceTemplateProvider.Terraform)
            .ToList();

        if (terraformProvisionInputs.Count > 0)
        {
            var terraformDriver = _serviceProvider.GetRequiredService<ITerraformDriver>();
            var terraformPlanInputs = terraformProvisionInputs
                .Select(tp => new TerraformPlanInput(tp.Template, tp.Inputs, tp.Key))
                .ToList();
            TerraformPlanResult planResult = await terraformDriver.PlanAsync(terraformPlanInputs, folderName);
            await terraformDriver.ApplyAsync(planResult);
        }
    }

    public async Task DeleteAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        switch (template.Provider)
        {
            case ResourceTemplateProvider.Terraform:
                var terraformDriver = _serviceProvider.GetRequiredService<ITerraformDriver>();
                var terraformPlanInput = new TerraformPlanInput(template, inputs, "");
                TerraformPlanDestroyResult planDestroyResult =
                    await terraformDriver.PlanDestroyAsync(terraformPlanInput);
                await terraformDriver.DestroyAsync(planDestroyResult);
                break;
            case ResourceTemplateProvider.Helm:
                var helmDriver = _serviceProvider.GetRequiredService<IHelmDriver>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}