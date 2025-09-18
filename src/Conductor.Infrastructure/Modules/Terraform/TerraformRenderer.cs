using System.Text;
using Conductor.Infrastructure.Modules.Terraform.Models;

namespace Conductor.Infrastructure.Modules.Terraform;

public interface ITerraformRenderer
{
    string RenderMainTf(List<TerraformPlanInput> terraformPlanInputs, string moduleDirectory);
    string RenderProvidersTf(List<TerraformProvider> providers);
}

public sealed class TerraformRenderer : ITerraformRenderer
{
    public string RenderMainTf(List<TerraformPlanInput> terraformPlanInputs, string moduleDirectory)
    {
        var sb = new StringBuilder();

        foreach (var terraformPlanInput in terraformPlanInputs)
        {
            var moduleName = terraformPlanInput.Template.Name.Replace(" ", "_").ToLowerInvariant();

            sb.AppendLine($"module \"{moduleName}\" {{");
            sb.AppendLine($"  source = \"{moduleDirectory}\"");

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