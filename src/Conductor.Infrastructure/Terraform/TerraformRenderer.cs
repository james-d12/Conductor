using System.Text;
using Conductor.Infrastructure.Terraform.Models;

namespace Conductor.Infrastructure.Terraform;

public interface ITerraformRenderer
{
    string RenderMainTf(Dictionary<TerraformPlanInput, TerraformValidationResult> terraformValidationResults);
    string RenderProvidersTf(List<TerraformProvider> providers);
}

public sealed class TerraformRenderer : ITerraformRenderer
{
    public string RenderMainTf(Dictionary<TerraformPlanInput, TerraformValidationResult> terraformValidationResults)
    {
        var sb = new StringBuilder();

        foreach (var (terraformPlanInput, terraformValidationResult) in terraformValidationResults)
        {
            var key = terraformPlanInput.Key.Replace(" ", "_").Trim().ToLowerInvariant();
            var templateName = terraformPlanInput.Template.Name.Replace(" ", "_").ToLowerInvariant();
            var moduleName = string.Join("_", templateName, key);

            sb.AppendLine($"module \"{moduleName}\" {{");
            sb.AppendLine($"  source = \"{terraformValidationResult.ModuleDirectory}\"");

            foreach (var kvp in terraformPlanInput.Inputs)
            {
                var value = QuoteIfNeeded(kvp.Value);
                sb.AppendLine($"  {kvp.Key} = {value}");
            }

            sb.AppendLine("}");
            sb.AppendLine("");
        }

        return sb.ToString();
    }

    public string RenderProvidersTf(List<TerraformProvider> providers)
    {
        var sb = new StringBuilder();
        sb.AppendLine("terraform {");
        sb.AppendLine("    required_providers {");

        foreach (var provider in providers)
        {
            sb.AppendLine($"      {provider.Name} = {{");
            sb.AppendLine($"         source = \"{provider.Source}\"");
            sb.AppendLine($"         version = \"{provider.Version}\"");
            sb.AppendLine("        }");
        }

        sb.AppendLine("     }");
        sb.AppendLine("}");

        foreach (var provider in providers)
        {
            sb.AppendLine($"provider \"{provider.Name}\" {{");
            sb.AppendLine("   features {}");
            sb.AppendLine("}");
        }

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