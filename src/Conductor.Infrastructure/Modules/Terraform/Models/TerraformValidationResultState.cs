namespace Conductor.Infrastructure.Modules.Terraform.Models;

public enum TerraformValidationResultState
{
    TemplateInvalid,
    ModuleInvalid,
    InputInvalid,
    Valid
}