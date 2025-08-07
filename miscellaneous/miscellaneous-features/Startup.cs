using Microsoft.Extensions.DependencyInjection;

namespace miscellaneous_features_playground;

public static class Startup
{
    internal static void ConfigureServices(IServiceCollection services)
    {
        services.AddHostedService<Principal>();
        // services.AddHostedService<MyBackgroundService>();
        // services.AddHostedService<MyHostedService>();

        services.AddScoped<StreamForEach>();
        services.AddScoped<InMemoryQueue>();
        services.AddScoped<Exceptions>();
    }
}