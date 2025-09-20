using Conductor.Core.Modules.Application.Domain;

namespace Conductor.Core.Modules.Application.Requests;

public sealed record CreateRepositoryRequest(string Name, Uri Url, RepositoryProvider Provider);