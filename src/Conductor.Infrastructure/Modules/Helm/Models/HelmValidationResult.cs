namespace Conductor.Infrastructure.Modules.Helm.Models;

public record HelmValidationResult
{
    public string Message { get; private init; } = string.Empty;
    public required HelmValidationResultState State { get; init; }

    public static HelmValidationResult Valid() => new()
    {
        State = HelmValidationResultState.Valid
    };

    public static HelmValidationResult WrongProvider(string message) => new()
    {
        Message = message,
        State = HelmValidationResultState.WrongProvider
    };

    public static HelmValidationResult TemplateNotFound(string message) => new()
    {
        Message = message,
        State = HelmValidationResultState.TemplateNotFound
    };

    public static HelmValidationResult ModuleNotFound(string message) => new()
    {
        Message = message,
        State = HelmValidationResultState.ModuleNotFound
    };

    public static HelmValidationResult ModuleNotParsable(string message) => new()
    {
        Message = message,
        State = HelmValidationResultState.ModuleNotParsable
    };

    public static HelmValidationResult InputNotPresent(string message) => new()
    {
        Message = message,
        State = HelmValidationResultState.InputNotPresent
    };

    public static HelmValidationResult RequiredInputNotProvided(string message) => new()
    {
        Message = message,
        State = HelmValidationResultState.RequiredInputNotProvided
    };
}