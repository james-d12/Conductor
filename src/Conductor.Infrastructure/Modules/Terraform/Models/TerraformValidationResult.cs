namespace Conductor.Infrastructure.Modules.Terraform.Models;

public record TerraformValidationResult
{
    public TerraformConfig? Config { get; init; }
    public string Message { get; private init; } = string.Empty;
    public required TerraformValidationResultState State { get; init; }

    public static TerraformValidationResult Valid(TerraformConfig config) => new()
    {
        Config = config,
        State = TerraformValidationResultState.Valid
    };

    public static TerraformValidationResult WrongProvider(string message) => new()
    {
        Message = message,
        State = TerraformValidationResultState.WrongProvider
    };

    public static TerraformValidationResult TemplateNotFound(string message) => new()
    {
        Message = message,
        State = TerraformValidationResultState.TemplateNotFound
    };

    public static TerraformValidationResult ModuleNotFound(string message) => new()
    {
        Message = message,
        State = TerraformValidationResultState.ModuleNotFound
    };

    public static TerraformValidationResult ModuleNotParsable(string message) => new()
    {
        Message = message,
        State = TerraformValidationResultState.ModuleNotParsable
    };

    public static TerraformValidationResult InputNotPresent(string message) => new()
    {
        Message = message,
        State = TerraformValidationResultState.InputNotPresent
    };

    public static TerraformValidationResult RequiredInputNotProvided(string message) => new()
    {
        Message = message,
        State = TerraformValidationResultState.RequiredInputNotProvided
    };
}