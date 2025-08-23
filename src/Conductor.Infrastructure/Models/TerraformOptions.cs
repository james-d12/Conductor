namespace Conductor.Infrastructure.Models;

public record TerraformOptions
{
    public required string TemporaryDirectory { get; init; }
}