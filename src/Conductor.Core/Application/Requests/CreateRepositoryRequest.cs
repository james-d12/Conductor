using Conductor.Core.Application.Domain;

namespace Conductor.Core.Application.Requests;

public sealed record CreateRepositoryRequest(string Name, Uri Url, RepositoryProvider Provider);