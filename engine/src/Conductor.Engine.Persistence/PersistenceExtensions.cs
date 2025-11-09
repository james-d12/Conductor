using Conductor.Engine.Domain.Application;
using Conductor.Engine.Domain.Deployment;
using Conductor.Engine.Domain.Environment;
using Conductor.Engine.Domain.ResourceTemplate;
using Conductor.Engine.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Conductor.Engine.Persistence;

public static class PersistenceExtensions
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

    public static async Task ApplyMigrations(this IServiceCollection services)
    {
        using IServiceScope scope = services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ConductorDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.MigrateAsync();
    }
}