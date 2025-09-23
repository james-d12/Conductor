using Conductor.Core.Provisioning;
using Conductor.Core.ResourceTemplate;
using Conductor.Infrastructure.Terraform;
using Conductor.Infrastructure.Terraform.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Conductor.Infrastructure.Resources;

public sealed class ResourceFactory : IProvisionFactory
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

    public async Task DeleteAsync(List<ProvisionInput> provisionInputs, string folderName)
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
            TerraformPlanResult planResult =
                await terraformDriver.PlanAsync(terraformPlanInputs, folderName, destroy: true);
            await terraformDriver.DestroyAsync(planResult);
        }
    }
}