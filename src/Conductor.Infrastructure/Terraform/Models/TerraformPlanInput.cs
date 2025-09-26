using Conductor.Domain.ResourceTemplate;

namespace Conductor.Infrastructure.Terraform.Models;

public sealed record TerraformPlanInput(ResourceTemplate Template, Dictionary<string, string> Inputs, string Key);