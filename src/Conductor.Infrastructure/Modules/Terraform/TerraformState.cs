using Conductor.Infrastructure.Modules.Terraform.Models;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Modules.Terraform;

public interface ITerraformState
{
    Task<string> SetupDirectoryAsync(
        TerraformPlanInput terraformPlanInput,
        TerraformValidationResult validationResult);
}

public sealed class TerraformState : ITerraformState
{
    private readonly ILogger<TerraformState> _logger;
    private readonly ITerraformRenderer _renderer;

    public TerraformState(ILogger<TerraformState> logger, ITerraformRenderer renderer)
    {
        _logger = logger;
        _renderer = renderer;
    }

    public async Task<string> SetupDirectoryAsync(
        TerraformPlanInput terraformPlanInput,
        TerraformValidationResult validationResult)
    {
        var stateDirectory = Path.Combine(Path.GetTempPath(), "conductor", "terraform", "state",
            terraformPlanInput.Template.Name.Replace(" ", "."));

        Directory.CreateDirectory(stateDirectory);

        var mainTf = _renderer.RenderMainTf([terraformPlanInput], validationResult.ModuleDirectory);
        _logger.LogDebug("Render output: {Output}", mainTf);
        var mainTfOutputPath = Path.Combine(stateDirectory, "main.tf");
        await File.WriteAllTextAsync(mainTfOutputPath, mainTf);
        _logger.LogInformation("Created main.tf to: {FilePath}", mainTfOutputPath);

        var providers = validationResult.Config?.RequiredProviders.Select(rp => new TerraformProvider(
            Name: rp.Key.ToString(),
            Source: rp.Value.Source,
            Version: rp.Value.VersionConstraints.FirstOrDefault() ?? string.Empty
        )).ToList();

        if (providers is null || providers.Count == 0)
        {
            throw new Exception($"No provider found for {terraformPlanInput.Template.Name} Passed.");
        }

        var providersTf = _renderer.RenderProvidersTf(providers);
        _logger.LogDebug("Render output: {Output}", mainTf);
        var providersTfOutputPath = Path.Combine(stateDirectory, "providers.tf");
        await File.WriteAllTextAsync(providersTfOutputPath, providersTf);
        _logger.LogInformation("Created providers.tf to: {FilePath}", providersTfOutputPath);

        return stateDirectory;
    }
}