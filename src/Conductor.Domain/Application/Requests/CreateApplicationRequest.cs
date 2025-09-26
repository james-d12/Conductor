namespace Conductor.Domain.Application.Requests;

public sealed record CreateApplicationRequest(string Name, CreateRepositoryRequest Repository);