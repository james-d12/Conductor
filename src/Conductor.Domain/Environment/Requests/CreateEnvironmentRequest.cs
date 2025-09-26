namespace Conductor.Domain.Environment.Requests;

public sealed record CreateEnvironmentRequest(string Name, string Description);