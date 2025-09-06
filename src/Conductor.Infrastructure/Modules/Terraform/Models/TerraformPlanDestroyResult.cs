using Conductor.Infrastructure.Common.CommandLine;

namespace Conductor.Infrastructure.Modules.Terraform.Models;

public sealed record TerraformPlanDestroyResult
{
    public string StateDirectory { get; init; }
    public CommandLineResult? PlanDestroyCommandLineResult { get; init; }

    public TerraformPlanDestroyResult()
    {
        StateDirectory = string.Empty;
        PlanDestroyCommandLineResult = null;
    }

    public TerraformPlanDestroyResult(string stateDirectory, CommandLineResult? planDestroyCommandLineResult = null)
    {
        StateDirectory = stateDirectory;
        PlanDestroyCommandLineResult = planDestroyCommandLineResult;
    }
}