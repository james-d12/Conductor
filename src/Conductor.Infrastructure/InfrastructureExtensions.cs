using Conductor.Core.Common.Services;
using Conductor.Infrastructure.Common;
using Conductor.Infrastructure.Modules.Terraform;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Conductor.Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TerraformOptions>()
            .Bind(configuration.GetSection("TerraformOptions"));
        services.TryAddSingleton<IResourceDriver, TerraformDriver>();
        services.TryAddSingleton<ITerraformRenderer, TerraformRenderer>();
        services.TryAddSingleton<ITerraformParser, TerraformParser>();
    }
}