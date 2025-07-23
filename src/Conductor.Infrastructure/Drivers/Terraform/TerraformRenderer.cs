using System.Text;
using Conductor.Domain.Models.ResourceTemplate;

namespace Conductor.Infrastructure.Drivers.Terraform;

public sealed class TerraformRenderer
{
    public string Render(ResourceTemplate template)
    {
        var moduleName = template.Name.Replace(" ", "_").ToLowerInvariant();
        var version = template.LatestVersion;

        if (version is null)
        {
            return string.Empty;
        }

        var source = version.Source.ToString();
        var inputs = version.Inputs ?? new Dictionary<string, string>();

        var sb = new StringBuilder();

        sb.AppendLine($"module \"{moduleName}\" {{");
        sb.AppendLine($"  source = \"{source}\"");

        foreach (var kvp in inputs)
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