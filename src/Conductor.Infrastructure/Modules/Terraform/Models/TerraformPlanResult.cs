using Conductor.Infrastructure.Common.CommandLine;

namespace Conductor.Infrastructure.Modules.Terraform.Models;

public sealed record TerraformPlanResult
{
    public string StateDirectory { get; init; }
    public CommandLineResult? PlanCommandLineResult { get; init; }

    public TerraformPlanResult()
    {
        StateDirectory = string.Empty;
        PlanCommandLineResult = null;
    }

    public TerraformPlanResult(string stateDirectory, CommandLineResult? planCommandLineResult = null)
    {
        StateDirectory = stateDirectory;
        PlanCommandLineResult = planCommandLineResult;
    }
}