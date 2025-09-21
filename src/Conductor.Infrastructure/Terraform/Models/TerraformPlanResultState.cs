namespace Conductor.Infrastructure.Terraform.Models;

public enum TerraformPlanResultState
{
    PreValidationFailed,
    InitFailed,
    ValidateFailed,
    PlanFailed,
    Success
}