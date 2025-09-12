namespace Conductor.Infrastructure.Modules.Terraform.Models;

public enum TerraformPlanResultState
{
    PreValidationFailed,
    InitFailed,
    ValidateFailed,
    PlanFailed,
    Success
}