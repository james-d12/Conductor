using Conductor.Core.Common.Services;
using Conductor.Infrastructure.Shared;
using Conductor.Infrastructure.Terraform;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Conductor.Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TerraformOptions>()
            .Bind(configuration.GetSection("TerraformOptions"));
        services.AddScoped<IResourceDriver, TerraformDriver>();
        services.AddScoped<ITerraformRenderer, TerraformRenderer>();
        services.AddScoped<ITerraformParser, TerraformParser>();
    }
}