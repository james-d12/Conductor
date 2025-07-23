using System.Text;
using Conductor.Domain.Models.ResourceTemplate;

namespace Conductor.Infrastructure.Drivers.Terraform;

public interface ITerraformRenderer
{
    string Render(ResourceTemplate template, Dictionary<string, string> actualInputs);
}

public sealed class TerraformRenderer : ITerraformRenderer
{
    public string Render(ResourceTemplate template, Dictionary<string, string> actualInputs)
    {
        var moduleName = template.Name.Replace(" ", "_").ToLowerInvariant();
        var version = template.LatestVersion;

        if (version is null)
        {
            return string.Empty;
        }

        foreach (var requiredInputKey in version.Inputs.Keys)
        {
            if (!actualInputs.ContainsKey(requiredInputKey))
            {
                throw new InvalidOperationException(
                    $"Missing required input '{requiredInputKey}' for template '{template.Name}'.");
            }
        }

        var sb = new StringBuilder();
        sb.AppendLine($"module \"{moduleName}\" {{");
        sb.AppendLine($"  source = \"{version.Source.ToString()}\"");

        foreach (var inputKey in version.Inputs.Keys)
        {
            var value = QuoteIfNeeded(actualInputs[inputKey]);
            sb.AppendLine($"  {inputKey} = {value}");
        }

        return sb.AppendLine("}").ToString();
    }

    private static string QuoteIfNeeded(string value)
    {
        if (bool.TryParse(value, out _) || int.TryParse(value, out _) || double.TryParse(value, out _))
        {
            return value;
        }

        return $"\"{value}\"";
    }
}