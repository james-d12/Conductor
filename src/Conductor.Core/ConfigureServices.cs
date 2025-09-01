using Conductor.Core.Common.Services;
using Conductor.Core.Modules.ResourceTemplate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Conductor.Core;

public static class ConfigureServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.TryAddScoped<IResourceTemplateService, ResourceTemplateService>();
        services.TryAddSingleton<IResourceDriverFactory, ResourceDriverFactory>();
        return services;
    }
}