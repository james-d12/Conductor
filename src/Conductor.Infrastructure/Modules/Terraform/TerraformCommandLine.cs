using Conductor.Infrastructure.Common.CommandLine;

namespace Conductor.Infrastructure.Modules.Terraform;

public interface ITerraformCommandLine
{
    Task<CommandLineResult> RunTerraformJsonOutput(string executeDirectory);
    Task<CommandLineResult> RunInitAsync(string executeDirectory);
    Task<CommandLineResult> RunValidateAsync(string executeDirectory);
    Task<CommandLineResult> RunPlanAsync(string executeDirectory);
}

public sealed class TerraformCommandLine : ITerraformCommandLine
{
    public async Task<CommandLineResult> RunTerraformJsonOutput(string executeDirectory) =>
        await new CommandLineBuilder("terraform-config-inspect")
            .WithArguments("--json .")
            .WithWorkingDirectory(executeDirectory)
            .ExecuteAsync();

    public async Task<CommandLineResult> RunInitAsync(string executeDirectory) =>
        await new CommandLineBuilder("terraform")
            .WithArguments("init")
            .WithWorkingDirectory(executeDirectory)
            .ExecuteAsync();

    public async Task<CommandLineResult> RunValidateAsync(string executeDirectory) =>
        await new CommandLineBuilder("terraform")
            .WithArguments("validate")
            .WithWorkingDirectory(executeDirectory)
            .ExecuteAsync();

    public async Task<CommandLineResult> RunPlanAsync(string executeDirectory) =>
        await new CommandLineBuilder("terraform")
            .WithArguments("plan -input=false -out=plan.tfplan")
            .WithWorkingDirectory(executeDirectory)
            .ExecuteAsync();
}