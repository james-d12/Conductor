namespace Conductor.Infrastructure.Shared;

public record TerraformOptions
{
    public required string TemporaryDirectory { get; init; }
}