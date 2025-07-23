using Conductor.Domain.Models.ResourceTemplate;

namespace Conductor.Domain.Services;

public sealed class TerraformDriver : IResourceDriver
{
    public string Name => "Terraform";

    public Task ValidateAsync(ResourceTemplateVersion version, Dictionary<string, string> inputs)
    {
        // clone version.Source, run `terraform validate`
        throw new NotImplementedException();
    }

    public Task PlanAsync(ResourceTemplateVersion version, Dictionary<string, string> inputs)
    {
        // run `terraform plan`
        throw new NotImplementedException();
    }

    public Task ApplyAsync(ResourceTemplateVersion version, Dictionary<string, string> inputs)
    {
        // run `terraform apply`
        throw new NotImplementedException();
    }

    public Task DestroyAsync(ResourceTemplateVersion version, Dictionary<string, string> inputs)
    {
        // run `terraform destroy`
        throw new NotImplementedException();
    }
}