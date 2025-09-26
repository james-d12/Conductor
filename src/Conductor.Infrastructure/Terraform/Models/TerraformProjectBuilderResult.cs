namespace Conductor.Infrastructure.Terraform.Models;

public sealed record TerraformProjectBuilderResult(string StateDirectory, string PlanDirectory);