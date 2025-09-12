using Conductor.Infrastructure.Common.CommandLine;

namespace Conductor.Infrastructure.Modules.Terraform.Models;

public sealed record TerraformPlanResult
{
    public string StateDirectory { get; init; }
    public string Message { get; init; }
    public int? ExitCode { get; init; }
    public TerraformPlanResultState State { get; init; }

    public TerraformPlanResult(TerraformPlanResultState state, string message)
    {
        State = state;
        Message = message;
        StateDirectory = string.Empty;
        ExitCode = null;
    }

    public TerraformPlanResult(string stateDirectory, TerraformPlanResultState state,
        CommandLineResult? planCommandLineResult = null)
    {
        StateDirectory = stateDirectory;
        Message = planCommandLineResult?.StdOut ?? planCommandLineResult?.StdErr ?? string.Empty;
        State = state;
        ExitCode = planCommandLineResult?.ExitCode;
    }
}