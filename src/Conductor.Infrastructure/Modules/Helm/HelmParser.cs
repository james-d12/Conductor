using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Modules.Helm;

public interface IHelmParser
{
    Task<object?> ParseHelmConfigAsync(string helmChartDirectory);
}

public sealed class HelmParser : IHelmParser
{
    private readonly ILogger<HelmParser> _logger;
    private readonly IHelmCommandLine _helmCommandLine;

    public HelmParser(ILogger<HelmParser> logger, IHelmCommandLine helmCommandLine)
    {
        _logger = logger;
        _helmCommandLine = helmCommandLine;
    }

    public async Task<object?> ParseHelmConfigAsync(string helmChartDirectory)
    {
        if (!IsValidModule(helmChartDirectory))
        {
            return null;
        }

        var inputJsonPath = Path.Combine(helmChartDirectory, "inputs-outputs.yaml");
        var createdJsonFile = await _helmCommandLine.GenerateOutputJsonAsync(helmChartDirectory, inputJsonPath);

        if (!createdJsonFile)
        {
            _logger.LogWarning("Could not create Input Json File: {File}", inputJsonPath);
            return null;
        }

        var fileContents = await File.ReadAllTextAsync(inputJsonPath);
        //File.Delete(inputJsonPath);
        return JsonSerializer.Deserialize<object>(fileContents);
    }

    private bool IsValidModule(string moduleDirectory)
    {
        var valuesFile = Directory
            .GetFiles(moduleDirectory, "values.yaml", SearchOption.AllDirectories)
            .FirstOrDefault();

        if (valuesFile is null)
        {
            _logger.LogWarning("Could not find values.yaml in template directory: {Directory} found.",
                moduleDirectory);
            return false;
        }

        return true;
    }
}