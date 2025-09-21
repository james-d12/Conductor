using Conductor.Core.Modules.Application;
using Conductor.Core.Modules.Deployment;
using Conductor.Core.Modules.Environment;
using Conductor.Core.Modules.ResourceTemplate;
using Conductor.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Conductor.Persistence;

public static class ConfigureServices
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        services.AddDbContext<ConductorDbContext>();
        services.AddScoped<IResourceTemplateRepository, ResourceTemplateRepository>();
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        services.AddScoped<IEnvironmentRepository, EnvironmentRepository>();
        services.AddScoped<IDeploymentRepository, DeploymentRepository>();

        return services;
    }

    public static async Task<IServiceCollection> ApplyMigrations(this IServiceCollection services)
    {
        using IServiceScope scope = services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ConductorDbContext>();
        await dbContext.Database.MigrateAsync();
        return services;
    }
}