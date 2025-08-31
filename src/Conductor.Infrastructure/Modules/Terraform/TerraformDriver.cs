using Conductor.Core.Common.Services;
using Conductor.Core.Modules.ResourceTemplate.Domain;
using Conductor.Infrastructure.Common;
using Conductor.Infrastructure.Modules.Terraform.Models;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Modules.Terraform;

public sealed class TerraformDriver : IResourceDriver
{
    public string Name => "Terraform";

    private readonly ILogger<TerraformDriver> _logger;
    private readonly ITerraformRenderer _renderer;
    private readonly ITerraformParser _parser;

    public TerraformDriver(ILogger<TerraformDriver> logger, ITerraformRenderer renderer, ITerraformParser parser)
    {
        _logger = logger;
        _renderer = renderer;
        _parser = parser;
    }

    public async Task ValidateAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        _logger.LogInformation("Validating Template: {Template} using the Terraform Driver.", template.Name);

        ResourceTemplateVersion? latestVersion = template.LatestVersion;

        if (latestVersion is null)
        {
            _logger.LogWarning("No Version could be found for {Template} found.", template.Name);
            return;
        }

        var templateDir = Path.Combine(Path.GetTempPath(), "conductor");
        var cloneResult =
            await GitCommandLine.CloneAsync(latestVersion.Source, templateDir, _logger, CancellationToken.None);

        if (!cloneResult)
        {
            _logger.LogWarning("Could not clone template: {Template} from {Source}", template.Name,
                latestVersion.Source.ToString());
            return;
        }

        _logger.LogInformation("Successfully cloned Repository: {Url} to {Output}", latestVersion.Source, templateDir);

        var variablesFile = Directory
            .GetFiles(templateDir, "variables.tf", SearchOption.AllDirectories)
            .FirstOrDefault();

        if (variablesFile is null)
        {
            _logger.LogWarning("Could not find variables.tf in template: {Template} found.", template.Name);
            return;
        }

        var localFile = new LocalFile
        {
            Name = "File",
            FullPath = variablesFile,
            Directory = Path.GetDirectoryName(variablesFile) ?? string.Empty
        };

        TerraformConfig? terraformConfig = await _parser.ParseTerraformModuleAsync(localFile);

        foreach (var variable in terraformConfig?.Variables ?? [])
        {
            _logger.LogInformation("Terraform Variable: {Variable}", variable.Key);
        }

        try
        {
            var mainTf = _renderer.Render(template, inputs);
            var outputPath = Path.Combine(templateDir, "conductor_main.tf");
            await File.WriteAllTextAsync(outputPath, mainTf);
            _logger.LogInformation("Written contents to: {FilePath}", outputPath);
        }
        finally
        {
            //Directory.Delete(tempDir, true);
        }
    }

    public Task PlanAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        // run `terraform plan`
        throw new NotImplementedException();
    }

    public Task ApplyAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        // run `terraform apply`
        throw new NotImplementedException();
    }

    public Task DestroyAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        // run `terraform destroy`
        throw new NotImplementedException();
    }
}