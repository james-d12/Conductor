using Conductor.Core.ResourceTemplate.Domain;

namespace Conductor.Infrastructure.Terraform.Models;

public sealed record TerraformPlanInput(ResourceTemplate Template, Dictionary<string, string> Inputs, string Key);