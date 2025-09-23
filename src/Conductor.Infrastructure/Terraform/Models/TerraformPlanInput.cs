using Conductor.Core.ResourceTemplate;

namespace Conductor.Infrastructure.Terraform.Models;

public sealed record TerraformPlanInput(ResourceTemplate Template, Dictionary<string, string> Inputs, string Key);