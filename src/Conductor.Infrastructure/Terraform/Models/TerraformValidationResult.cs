namespace Conductor.Infrastructure.Terraform.Models;

public sealed record TerraformValidationResult
{
    public TerraformConfig? Config { get; init; }
    public string Message { get; private init; } = string.Empty;
    public string ModuleDirectory { get; private init; } = string.Empty;
    public required TerraformValidationResultState State { get; init; }

    public static TerraformValidationResult Valid(TerraformConfig config, string moduleDirectory) => new()
    {
        Config = config,
        ModuleDirectory = moduleDirectory,
        State = TerraformValidationResultState.Valid
    };

    public static TerraformValidationResult TemplateInvalid(string message) => new()
    {
        Message = message,
        State = TerraformValidationResultState.TemplateInvalid
    };

    public static TerraformValidationResult ModuleInvalid(string message) => new()
    {
        Message = message,
        State = TerraformValidationResultState.ModuleInvalid
    };

    public static TerraformValidationResult InputInvalid(string message) => new()
    {
        Message = message,
        State = TerraformValidationResultState.InputInvalid
    };
}