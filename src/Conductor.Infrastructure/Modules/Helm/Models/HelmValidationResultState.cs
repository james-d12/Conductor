namespace Conductor.Infrastructure.Modules.Helm.Models;

public enum HelmValidationResultState
{
    WrongProvider,
    TemplateNotFound,
    ModuleNotFound,
    ModuleNotParsable,
    InputNotPresent,
    RequiredInputNotProvided,
    Valid
}