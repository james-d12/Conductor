using Conductor.Core.Provisioning;
using Conductor.Core.Provisioning.Requirements;
using Conductor.Infrastructure.CommandLine;
using Conductor.Infrastructure.Helm;
using Conductor.Infrastructure.Resources;
using Conductor.Infrastructure.Score;
using Conductor.Infrastructure.Terraform;
using Conductor.Infrastructure.Terraform.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Conductor.Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSharedServices();
        services.AddScoreServices();
        services.AddHelmServices();
        services.AddTerraformServices(configuration);
    }

    private static void AddSharedServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IGitCommandLine, GitCommandLine>();
        services.TryAddScoped<IProvisionFactory, ResourceFactory>();
    }

    private static void AddScoreServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IScoreParser, ScoreParser>();
        services.TryAddSingleton<IScoreValidator, ScoreValidator>();
        services.TryAddSingleton<IRequirementDriver, ScoreRequirementDriver>();
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