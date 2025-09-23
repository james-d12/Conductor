namespace Conductor.Core.Provisioning;

public sealed record ProvisionInput(
    ResourceTemplate.ResourceTemplate Template,
    Dictionary<string, string> Inputs,
    string Key);