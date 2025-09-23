using Conductor.Core.Provisioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Conductor.Core;

public static class ConfigureServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.TryAddScoped<ProvisioningService>();
        return services;
    }
}