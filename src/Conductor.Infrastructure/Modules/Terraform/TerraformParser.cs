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
        if (!IsValidModule(moduleDirectory))
        {
            return null;
        }

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

    private bool IsValidModule(string moduleDirectory)
    {
        var variablesFile = Directory
            .GetFiles(moduleDirectory, "variables.tf", SearchOption.AllDirectories)
            .FirstOrDefault();

        if (variablesFile is null)
        {
            _logger.LogWarning("Could not find variables.tf in template directory: {Directory} found.",
                moduleDirectory);
            return false;
        }

        var outputsFile = Directory
            .GetFiles(moduleDirectory, "outputs.tf", SearchOption.AllDirectories)
            .FirstOrDefault();

        if (outputsFile is null)
        {
            _logger.LogWarning("Could not find outputs.tf in template directory: {Directory} found.", moduleDirectory);
            return false;
        }

        return true;
    }
}