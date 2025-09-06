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
        var inputJsonPath = Path.Combine(moduleDirectory, "inputs-outputs.json");
        var createdJsonFile = await _terraformCommandLine.GenerateOutputJsonAsync(moduleDirectory, inputJsonPath);

        if (!createdJsonFile)
        {
            _logger.LogWarning("Could not create Input Json File: {File}", inputJsonPath);
            return null;
        }

        var fileContents = await File.ReadAllTextAsync(inputJsonPath);
        File.Delete(inputJsonPath);
        return JsonSerializer.Deserialize<TerraformConfig>(fileContents);
    }
}