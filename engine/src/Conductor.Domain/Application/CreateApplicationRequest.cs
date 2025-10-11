namespace Conductor.Domain.Application;

public sealed record CreateApplicationRequest(string Name, CreateRepositoryRequest Repository);