namespace Conductor.Infrastructure.Modules.Terraform.Models;

public record TerraformOptions
{
    public required string TemporaryDirectory { get; init; }
}