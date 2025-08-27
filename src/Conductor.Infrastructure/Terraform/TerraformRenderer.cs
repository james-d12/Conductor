using System.Text;
using Conductor.Domain.Models;

namespace Conductor.Infrastructure.Terraform;

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

        var sb = new StringBuilder();
        sb.AppendLine($"module \"{moduleName}\" {{");
        sb.AppendLine($"  source = \"{version.Source.ToString()}\"");

        foreach (var kvp in actualInputs)
        {
            var value = QuoteIfNeeded(kvp.Value);
            sb.AppendLine($"  {kvp.Key} = {value}");
        }

        sb.AppendLine("}");
        return sb.ToString();
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