namespace Conductor.Infrastructure.Terraform.Models;

public enum TerraformValidationResultState
{
    TemplateInvalid,
    ModuleInvalid,
    InputInvalid,
    Valid
}