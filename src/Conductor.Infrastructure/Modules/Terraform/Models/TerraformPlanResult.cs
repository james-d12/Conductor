using Conductor.Infrastructure.Common.CommandLine;

namespace Conductor.Infrastructure.Modules.Terraform.Models;

public sealed record TerraformPlanResult
{
    public required string StateDirectory { get; init; }
    public required TerraformValidationResult ValidationResult { get; init; }
    public required CommandLineResult CommandLineResult { get; init; }
}