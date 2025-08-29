using Conductor.Core.Modules.ResourceTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace Conductor.Core;

public static class ConfigureServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IResourceTemplateService, ResourceTemplateService>();
        return services;
    }
}