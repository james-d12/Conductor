using System.Text;
using Conductor.Core.Modules.ResourceTemplate.Domain;

namespace Conductor.Infrastructure.Modules.Terraform;

public interface ITerraformRenderer
{
    string Render(ResourceTemplate template, string moduleDirectory,
        Dictionary<string, string> actualInputs);
}

public sealed class TerraformRenderer : ITerraformRenderer
{
    public string Render(ResourceTemplate template, string moduleDirectory,
        Dictionary<string, string> actualInputs)
    {
        var moduleName = template.Name.Replace(" ", "_").ToLowerInvariant();

        var sb = new StringBuilder();
        sb.AppendLine($"module \"{moduleName}\" {{");
        sb.AppendLine($"  source = \"{moduleDirectory}\"");

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

        if (value.StartsWith('[') && value.EndsWith(']'))
        {
            return value.Replace("\'", "\"");
        }

        return $"\"{value}\"";
    }
}