namespace Conductor.Infrastructure.Modules.Helm.Models;

public sealed record HelmInput(string Key, object? DefaultValue);