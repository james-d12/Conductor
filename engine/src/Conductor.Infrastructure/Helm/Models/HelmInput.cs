namespace Conductor.Infrastructure.Helm.Models;

public sealed record HelmInput(string Key, object? DefaultValue);