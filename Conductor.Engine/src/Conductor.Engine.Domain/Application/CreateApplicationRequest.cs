namespace Conductor.Engine.Domain.Application;

public sealed record CreateApplicationRequest(string Name, CreateRepositoryRequest Repository);