using System.Text.Json;
using Conductor.Infrastructure.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Conductor.Infrastructure.Terraform;

public interface ITerraformParser
{
    Task<Dictionary<string, string>> ParseInputsAsync(LocalFile inputFile);
    Task<Dictionary<string, string>> ParseOutputsAsync(LocalFile outputFile);
}

public sealed class TerraformParser : ITerraformParser
{
    private const string InputFileName = "variables.tf";
    private const string OutputFileName = "outputs.tf";
    private readonly TerraformOptions _options;

    private readonly ILogger<TerraformParser> _logger;

    public TerraformParser(ILogger<TerraformParser> logger, IOptions<TerraformOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public async Task<Dictionary<string, string>> ParseInputsAsync(LocalFile inputFile)
    {
        if (!inputFile.Name.Equals(inputFile.Name, StringComparison.InvariantCultureIgnoreCase))
        {
            _logger.LogWarning("File: {Filename} does not match: {InputFileName}", inputFile.Name, InputFileName);
            return [];
        }

        var inputJsonPath = Path.Combine(_options.TemporaryDirectory, $"{Guid.NewGuid().ToString()}-inputs.json");
        var createdJsonFile =
            await TerraformCommandLine.GenerateOutputJsonAsync(inputFile.Directory, inputJsonPath, _logger);

        if (!createdJsonFile)
        {
            _logger.LogWarning("Could not create Input Json File: {File}", inputJsonPath);
            return [];
        }

        var fileContents = await File.ReadAllTextAsync(inputJsonPath);
        var input = JsonDocument.Parse(fileContents);

        return [];
    }

    public async Task<Dictionary<string, string>> ParseOutputsAsync(LocalFile outputFile)
    {
        if (!outputFile.Name.Equals(outputFile.Name, StringComparison.InvariantCultureIgnoreCase))
        {
            _logger.LogWarning("File: {Filename} does not match: {OutputFileName}", outputFile.Name, OutputFileName);
            return [];
        }

        var outputJsonPath = Path.Combine(_options.TemporaryDirectory, $"{Guid.NewGuid().ToString()}-outputs.json");
        var createdJsonFile =
            await TerraformCommandLine.GenerateOutputJsonAsync(outputFile.Directory, outputJsonPath, _logger);

        if (!createdJsonFile)
        {
            _logger.LogWarning("Could not create Output Json File: {File}", outputJsonPath);
            return [];
        }

        var fileContents = await File.ReadAllTextAsync(outputJsonPath);
        var output = JsonDocument.Parse(fileContents);

        return [];
    }
}