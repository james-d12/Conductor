namespace Conductor.Domain.ResourceTemplate.Domain;

public sealed record ResourceTemplateVersionSource
{
    public required Uri BaseUrl { get; init; }
    public required string FolderPath { get; init; }
    public required string Tag { get; init; }
}