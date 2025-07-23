using System.Diagnostics;
using Conductor.Domain.Models.ResourceTemplate;
using Conductor.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Conductor.Infrastructure.Drivers.Terraform;

public sealed class TerraformDriver : IResourceDriver
{
    public string Name => "Terraform";

    private readonly ILogger<TerraformDriver> _logger;
    private readonly ITerraformRenderer _renderer;

    public TerraformDriver(ILogger<TerraformDriver> logger, ITerraformRenderer renderer)
    {
        _logger = logger;
        _renderer = renderer;
    }

    public async Task ValidateAsync(ResourceTemplate template, Dictionary<string, string> inputs)
    {
        _logger.LogInformation("Validating Template: {Template} using the Terraform Driver.", template.Name);
        var tempDir = Path.Combine(Path.GetTempPath(), "conductor-test");
        Directory.CreateDirectory(tempDir);

        try
        {
            // 1. Write module block
            var mainTf = _renderer.Render(template, inputs);
            await File.WriteAllTextAsync(Path.Combine(tempDir, "main.tf"), mainTf);

            _logger.LogInformation("Written contents to: {FilePath}", tempDir + "/main.tf");
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

    private static async Task RunTerraformCommand(string command, string workingDirectory)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "terraform",
                Arguments = command,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };

        process.Start();
        var output = await process.StandardOutput.ReadToEndAsync();
        var error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"Terraform {command} failed:\n{error}");
        }
    }
}