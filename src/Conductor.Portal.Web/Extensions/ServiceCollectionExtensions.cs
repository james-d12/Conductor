using MudBlazor.Services;

namespace Conductor.Portal.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterCoreServices(this IServiceCollection services)
    {
        services.AddRazorComponents().AddInteractiveServerComponents();
        services.AddMudServices();
        services.AddMemoryCache(options => options.TrackStatistics = true);
    }
}