using Conductor.Portal.Web.Components;
using Conductor.Portal.Web.Extensions;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
var applicationName = AppDomain.CurrentDomain.FriendlyName;

var loggerFactory = LoggerFactory.Create(loggingBuilder =>
{
    loggingBuilder.AddJsonConsole();
    loggingBuilder.AddConfiguration(builder.Configuration.GetSection("Logging"));
    loggingBuilder.AddDebug();
    loggingBuilder.AddEventSourceLogger();
});
var logger = loggerFactory.CreateLogger<Program>();

try
{
    logger.LogInformation("Starting: {ApplicationName}", applicationName);

    builder.Services.RegisterCoreServices();

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseHsts();
    }

    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedHost |
                           ForwardedHeaders.XForwardedProto
    });

    app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseAntiforgery();

    app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

    app.Run();
}
catch (Exception exception)
{
    logger.LogCritical(exception, "Could not startup: {ApplicationName}.", applicationName);
    throw;
}
finally
{
    logger.LogInformation("Stopping: {ApplicationName}.", applicationName);
}