namespace Conductor.Infrastructure.Modules.Terraform.Models;

public sealed record TerraformProvider(string Name, string Source, string Version);