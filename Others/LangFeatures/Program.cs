using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;

namespace LangFeatures_Sample
{
    public static class Program
    {
        private static IHost _host;
        static void Main(string[] args)
        {
            _host = CreateHostBuilder(args).Build();
            _host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => logging
                        .ClearProviders()
                        .AddFilter("Microsoft", LogLevel.Error)
                        .AddFilter("System", LogLevel.Error)
                        .AddFilter("LangFeatures_Sample", LogLevel.Debug)
                        .AddFilter<DebugLoggerProvider>("Microsoft", LogLevel.Error)
                        .AddFilter<ConsoleLoggerProvider>("Microsoft", LogLevel.Error)
                        .AddSimpleConsole(opts =>
                        {
                            opts.IncludeScopes = true;
                            opts.SingleLine = true;
                            opts.TimestampFormat = "dd/MM/yyyy HH:mm:ss.fff ";
                        }))
                .ConfigureServices((context, services) =>
                {
                    ServiceCollectionHostedServiceExtensions.AddHostedService<Principal>(services);
                    Startup.ConfigureServices(services);
                });
    }
}
