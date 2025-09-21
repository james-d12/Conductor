namespace Conductor.Infrastructure.Terraform.Models;

public record TerraformOptions
{
    public required string TemporaryDirectory { get; init; }
}