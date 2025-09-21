using System.Text.Json.Serialization;

namespace Conductor.Infrastructure.Terraform.Models;

public record TerraformConfig
{
    [JsonPropertyName("path")]
    public string Path { get; init; } = string.Empty;

    [JsonPropertyName("variables")]
    public Dictionary<string, Variable> Variables { get; init; } = [];

    [JsonPropertyName("outputs")]
    public Dictionary<string, Output> Outputs { get; init; } = [];

    [JsonPropertyName("required_core")]
    public List<string> RequiredCore { get; init; } = [];

    [JsonPropertyName("required_providers")]
    public Dictionary<string, RequiredProvider> RequiredProviders { get; init; } = [];

    [JsonPropertyName("managed_resources")]
    public Dictionary<string, object> ManagedResources { get; init; } = [];

    [JsonPropertyName("data_resources")]
    public Dictionary<string, object> DataResources { get; init; } = [];

    [JsonPropertyName("module_calls")]
    public Dictionary<string, ModuleCall> ModuleCalls { get; init; } = [];
}

public record Variable
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("default")]
    public object? Default { get; set; }

    [JsonPropertyName("required")]
    public bool Required { get; set; }

    [JsonPropertyName("pos")]
    public Position? Pos { get; set; }
}

public record Output
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("pos")]
    public Position? Pos { get; set; }
}

public record RequiredProvider
{
    [JsonPropertyName("source")]
    public string Source { get; set; } = string.Empty;

    [JsonPropertyName("version_constraints")]
    public List<string> VersionConstraints { get; set; } = [];
}

public record ModuleCall
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("version")]
    public string? Version { get; set; }

    [JsonPropertyName("pos")]
    public Position? Pos { get; set; }
}

public record Position
{
    [JsonPropertyName("filename")]
    public string Filename { get; set; } = string.Empty;

    [JsonPropertyName("line")]
    public int Line { get; set; }
}