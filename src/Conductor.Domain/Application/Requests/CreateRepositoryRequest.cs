using Conductor.Domain.Application.Domain;

namespace Conductor.Domain.Application.Requests;

public sealed record CreateRepositoryRequest(string Name, Uri Url, RepositoryProvider Provider);