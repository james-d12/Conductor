namespace Conductor.Core.Modules.Application.Requests;

public sealed record CreateApplicationRequest(string Name, CreateRepositoryRequest Repository);