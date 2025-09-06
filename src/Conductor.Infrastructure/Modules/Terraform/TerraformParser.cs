using System.Text.Json;
using Conductor.Infrastructure.Modules.Terraform.Models;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Modules.Terraform;

public interface ITerraformParser
{
    Task<TerraformConfig?> ParseTerraformModuleAsync(string moduleDirectory);
}

public sealed class TerraformParser : ITerraformParser
{
    private readonly ILogger<TerraformParser> _logger;
    private readonly ITerraformCommandLine _terraformCommandLine;

    public TerraformParser(ILogger<TerraformParser> logger, ITerraformCommandLine terraformCommandLine)
    {
        _logger = logger;
        _terraformCommandLine = terraformCommandLine;
    }

    public async Task<TerraformConfig?> ParseTerraformModuleAsync(string moduleDirectory)
    {
        var runTerraformJsonOutput = await _terraformCommandLine.RunTerraformJsonOutput(moduleDirectory);

        if (runTerraformJsonOutput.ExitCode != 0)
        {
            _logger.LogWarning("Could not get json output for {Module}", moduleDirectory);
            return null;
        }

        return JsonSerializer.Deserialize<TerraformConfig>(runTerraformJsonOutput.StdOut);
    }
}