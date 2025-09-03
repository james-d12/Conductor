using Conductor.Core.Common.Services;
using Conductor.Infrastructure.Common;
using Conductor.Infrastructure.Modules.Helm;
using Conductor.Infrastructure.Modules.Terraform;
using Conductor.Infrastructure.Modules.Terraform.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Conductor.Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TerraformOptions>()
            .Bind(configuration.GetSection(nameof(TerraformOptions)));

        services.AddSingleton<IResourceDriver, TerraformDriver>();
        services.AddSingleton<IResourceDriver, HelmDriver>();

        services.TryAddSingleton<IGitCommandLine, GitCommandLine>();
        services.TryAddSingleton<ITerraformRenderer, TerraformRenderer>();
        services.TryAddSingleton<ITerraformParser, TerraformParser>();
        services.TryAddSingleton<ITerraformCommandLine, TerraformCommandLine>();
        services.TryAddSingleton<ITerraformValidator, TerraformValidator>();
    }
}