namespace Conductor.Infrastructure.Modules.Terraform.Models;

public enum TerraformValidationResultState
{
    WrongProvider,
    TemplateNotFound,
    ModuleNotFound,
    ModuleNotParsable,
    InputNotPresent,
    RequiredInputNotProvided,
    Valid
}