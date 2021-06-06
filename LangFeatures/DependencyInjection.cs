using System;
using Microsoft.Extensions.DependencyInjection;

namespace LangFeatures_Sample
{
    public static class DependencyInjection
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static void Configure()
        {
            var services = new ServiceCollection();
            ConfigreServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        private static void ConfigreServices(ServiceCollection services)
        {
            //services.AddConsoleLog();
        }
    }
}
