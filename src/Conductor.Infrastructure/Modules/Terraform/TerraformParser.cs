using System.Text.Json;
using Conductor.Infrastructure.Common;
using Conductor.Infrastructure.Modules.Terraform.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Conductor.Infrastructure.Modules.Terraform;

public interface ITerraformParser
{
    Task<TerraformConfig?> ParseTerraformModuleAsync(LocalFile inputFile);
}

public sealed class TerraformParser : ITerraformParser
{
    private const string InputFileName = "variables.tf";
    private readonly TerraformOptions _options;

    private readonly ILogger<TerraformParser> _logger;

    public TerraformParser(ILogger<TerraformParser> logger, IOptions<TerraformOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public async Task<TerraformConfig?> ParseTerraformModuleAsync(LocalFile inputFile)
    {
        if (!inputFile.Name.Equals(inputFile.Name, StringComparison.InvariantCultureIgnoreCase))
        {
            _logger.LogWarning("File: {Filename} does not match: {InputFileName}", inputFile.Name, InputFileName);
            return null;
        }

        var inputJsonPath = Path.Combine(_options.TemporaryDirectory, $"{Guid.NewGuid()}-inputs.json");
        var createdJsonFile =
            await TerraformCommandLine.GenerateOutputJsonAsync(inputFile.Directory, inputJsonPath, _logger);

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