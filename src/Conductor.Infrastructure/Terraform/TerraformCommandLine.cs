using System.Diagnostics;

namespace Conductor.Infrastructure.Terraform;

public static class TerraformCommandLine
{
    public static async Task<bool> GenerateInputJsonAsync(string executeDirectory, string outputJsonPath)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "terraform",
            Arguments = $"output -json > \'{outputJsonPath}'\"",
            WorkingDirectory = executeDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = startInfo;
        process.Start();
        await process.WaitForExitAsync();
        return File.Exists(outputJsonPath);
    }

    public static async Task<bool> GenerateOutputJsonAsync(string executeDirectory, string outputJsonPath)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "terraform",
            Arguments = $"output -json > \'{outputJsonPath}'\"",
            WorkingDirectory = executeDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = startInfo;
        process.Start();
        await process.WaitForExitAsync();
        return File.Exists(outputJsonPath);
    }
}