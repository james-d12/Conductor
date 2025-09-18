using Conductor.Core.Modules.ResourceTemplate.Domain;

namespace Conductor.Infrastructure.Modules.Terraform.Models;

public sealed record TerraformPlanInput(ResourceTemplate Template, Dictionary<string, string> Inputs, string Key);