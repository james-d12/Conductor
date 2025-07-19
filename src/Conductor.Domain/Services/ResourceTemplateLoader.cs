using System.Text.Json;
using Scriban;
using YamlDotNet.Serialization;

namespace Conductor.Domain.Services;

public sealed class ResourceTemplateLoader
{
    public void Run()
    {
        var devConfig = LoadYaml<DeveloperConfig>("developer-config.yaml");
        var secrets = new Dictionary<string, string> { { "POSTGRES_PASSWORD", "supersecret123" } };

        foreach (var res in devConfig.Resources)
        {
            var mapPath = $"{res.Type}.yaml";
            var mapping = LoadYaml<ModuleMapping>(mapPath);

            var renderEnv = new
            {
                app_name = devConfig.Name,
                env = devConfig.Env,
                secrets = secrets,
                config = res.Config
            };

            var renderedInputs = new Dictionary<string, object>();
            foreach (var entry in mapping.Inputs)
            {
                var template = Template.Parse(entry.Value);
                var value = template.Render(renderEnv, member => member.Name);
                renderedInputs[entry.Key] = TryConvert(value);
            }

            var outputDir = Path.Combine("orchestrator-output", res.Type);
            Directory.CreateDirectory(outputDir);

            File.WriteAllText(Path.Combine(outputDir, "main.tf"),
                GenerateMainTf(mapping.Source, renderedInputs.Keys.ToList()));
            File.WriteAllText(Path.Combine(outputDir, "variables.tf"),
                GenerateVariablesTf(renderedInputs.Keys.ToList()));
            var json = JsonSerializer.Serialize(renderedInputs, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path.Combine(outputDir, "terraform.tfvars.json"), json);
        }
    }

    private T LoadYaml<T>(string path)
    {
        var yaml = File.ReadAllText(path);
        var deserializer = new Deserializer();
        return deserializer.Deserialize<T>(yaml);
    }

    private string GenerateMainTf(string source, List<string> inputs)
    {
        var inputLines = inputs.Select(i => $"  {i} = var.{i}");
        var varsBlock = string.Join("\n", inputLines);

        return
            $$"""
              module "resource" {
                source = "{{source}}"
                {{varsBlock}}
              }
              """;
    }

    private string GenerateVariablesTf(List<string> inputs)
    {
        var vars = inputs.Select(i =>
            $@"variable ""{i}"" {{
  type = any
}}");

        return string.Join("\n\n", vars);
    }

    private object TryConvert(string value)
    {
        // Try to convert string to int if applicable
        return int.TryParse(value, out var i) ? i : value;
    }
}

class DeveloperConfig
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; }

    [YamlMember(Alias = "env")]
    public string Env { get; set; }

    [YamlMember(Alias = "resources")]
    public List<DeveloperResource> Resources { get; set; }
}

class DeveloperResource
{
    [YamlMember(Alias = "type")]
    public string Type { get; set; }

    [YamlMember(Alias = "config")]
    public Dictionary<string, object> Config { get; set; }
}

class ModuleMapping
{
    [YamlMember(Alias = "type")]
    public string Type { get; set; }

    [YamlMember(Alias = "source")]
    public string Source { get; set; }

    [YamlMember(Alias = "inputs")]
    public Dictionary<string, string> Inputs { get; set; }
}