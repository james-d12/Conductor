using Conductor.Infrastructure.CommandLine;
using Conductor.Infrastructure.Helm;
using Conductor.Infrastructure.Resources;
using Conductor.Infrastructure.Score;
using Conductor.Infrastructure.Terraform;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Conductor.Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSharedServices();
        services.AddScoreServices();
        services.AddHelmServices();
        services.AddTerraformServices();
    }

    private static void AddSharedServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IGitCommandLine, GitCommandLine>();
        services.TryAddSingleton<IResourceFactory, ResourceFactory>();
        services.TryAddScoped<IResourceProvisioner, ResourceProvisioner>();
    }

    private static void AddScoreServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IScoreDriver, ScoreDriver>();
    }

    private static void AddHelmServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IHelmDriver, HelmDriver>();
        services.TryAddSingleton<IHelmValidator, HelmValidator>();
        services.TryAddSingleton<IHelmParser, HelmParser>();
    }

    private static void AddTerraformServices(this IServiceCollection services)
    {
        services.TryAddSingleton<ITerraformDriver, TerraformDriver>();
        services.TryAddSingleton<ITerraformProjectBuilder, TerraformProjectBuilder>();
        services.TryAddSingleton<ITerraformRenderer, TerraformRenderer>();
        services.TryAddSingleton<ITerraformCommandLine, TerraformCommandLine>();
        services.TryAddSingleton<ITerraformValidator, TerraformValidator>();
    }
}