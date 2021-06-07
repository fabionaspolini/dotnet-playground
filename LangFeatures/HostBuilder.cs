using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;

namespace LangFeatures_Sample
{
    class HostBuilder
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
                        .AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Debug)
                        .AddFilter("LangFeatures_Sample", LogLevel.Debug)
                        .AddFilter<DebugLoggerProvider>("Microsoft", LogLevel.Information)
                        .AddFilter<ConsoleLoggerProvider>("Microsoft", LogLevel.Trace)
                        .AddSimpleConsole(opts =>
                        {
                            opts.IncludeScopes = true;
                            opts.SingleLine = true;
                            opts.TimestampFormat = "dd/MM/yyyy HH:mm:ss.fff ";
                        }))
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<Program>();
                    Startup.ConfigureServices(services);
                });
    }
}
