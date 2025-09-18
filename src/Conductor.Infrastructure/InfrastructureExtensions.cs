using Conductor.Infrastructure.Common.CommandLine;
using Conductor.Infrastructure.Modules.Helm;
using Conductor.Infrastructure.Modules.Score;
using Conductor.Infrastructure.Modules.Terraform;
using Conductor.Infrastructure.Modules.Terraform.Models;
using Conductor.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Conductor.Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddSingleton<IGitCommandLine, GitCommandLine>();

        services.AddSharedServices();
        services.AddHelmServices();
        services.AddTerraformServices(configuration);
    }

    private static void AddSharedServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IResourceFactory, ResourceFactory>();
        services.TryAddSingleton<IScoreParser, ScoreParser>();
        services.TryAddSingleton<ResourceProvisioner>();
    }

    private static void AddHelmServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IHelmDriver, HelmDriver>();
        services.TryAddSingleton<IHelmValidator, HelmValidator>();
        services.TryAddSingleton<IHelmParser, HelmParser>();
    }

    private static void AddTerraformServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TerraformOptions>()
            .Bind(configuration.GetSection(nameof(TerraformOptions)));

        services.TryAddSingleton<ITerraformDriver, TerraformDriver>();
        services.TryAddSingleton<ITerraformProjectBuilder, TerraformProjectBuilder>();
        services.TryAddSingleton<ITerraformRenderer, TerraformRenderer>();
        services.TryAddSingleton<ITerraformCommandLine, TerraformCommandLine>();
        services.TryAddSingleton<ITerraformValidator, TerraformValidator>();
    }
}