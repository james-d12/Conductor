using Conductor.Domain.Services;
using Conductor.Infrastructure.Drivers.Terraform;
using Microsoft.Extensions.DependencyInjection;

namespace Conductor.Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IResourceDriver, TerraformDriver>();
        services.AddScoped<ITerraformRenderer, TerraformRenderer>();
    }
}