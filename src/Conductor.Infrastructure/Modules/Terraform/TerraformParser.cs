using System.Text.Json;
using Conductor.Infrastructure.Common;
using Conductor.Infrastructure.Modules.Terraform.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Conductor.Infrastructure.Modules.Terraform;

public interface ITerraformParser
{
    Task<TerraformConfig?> ParseTerraformModuleAsync(string moduleDirectory);
}

public sealed class TerraformParser : ITerraformParser
{
    private readonly TerraformOptions _options;

    private readonly ILogger<TerraformParser> _logger;

    public TerraformParser(ILogger<TerraformParser> logger, IOptions<TerraformOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public async Task<TerraformConfig?> ParseTerraformModuleAsync(string moduleDirectory)
    {
        if (!IsValidModule(moduleDirectory))
        {
            return null;
        }

        var inputJsonPath = Path.Combine(_options.TemporaryDirectory, $"{Guid.NewGuid()}-inputs.json");
        var createdJsonFile =
            await TerraformCommandLine.GenerateOutputJsonAsync(moduleDirectory, inputJsonPath, _logger);

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