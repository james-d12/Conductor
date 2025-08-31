namespace Conductor.Infrastructure.Common;

public record TerraformOptions
{
    public required string TemporaryDirectory { get; init; }
}