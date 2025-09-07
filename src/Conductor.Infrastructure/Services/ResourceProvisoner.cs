using Conductor.Core.Modules.ResourceTemplate.Domain;
using Conductor.Infrastructure.Modules.Helm;
using Conductor.Infrastructure.Modules.Terraform;
using Conductor.Infrastructure.Modules.Terraform.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Conductor.Infrastructure.Services;

public interface IResourceProvisioner
{
    Task ProvisionAsync(ResourceTemplate template, Dictionary<string, string> inputs);
    Task DeleteAsync(ResourceTemplate template, Dictionary<string, string> inputs);
}

public sealed class ResourceProvisioner : IResourceProvisioner
{
    private readonly IServiceProvider _serviceProvider;

    public ResourceProvisioner(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ProvisionAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        switch (template.Provider)
        {
            case ResourceTemplateProvider.Terraform:
                var terraformDriver = _serviceProvider.GetRequiredService<ITerraformDriver>();
                TerraformPlanResult planResult = await terraformDriver.PlanAsync(template, inputs);
                await terraformDriver.ApplyAsync(planResult);
                break;
            case ResourceTemplateProvider.Helm:
                var helmDriver = _serviceProvider.GetRequiredService<IHelmDriver>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async Task DeleteAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        switch (template.Provider)
        {
            case ResourceTemplateProvider.Terraform:
                var terraformDriver = _serviceProvider.GetRequiredService<ITerraformDriver>();
                TerraformPlanDestroyResult planDestroyResult = await terraformDriver.PlanDestroyAsync(template, inputs);
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