using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.Diagnostics;
using static System.Console;

namespace NLogAsMicrosoftProxy_Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine(".:: NLog ::.");
            var nlogger = LogManager.GetCurrentClassLogger();
            try
            {
                Trace.CorrelationManager.ActivityId = Guid.NewGuid();

                var config = new ConfigurationBuilder().Build();
                var serviceProvider = BuildDi(config);

                using var scope = serviceProvider.CreateScope();
                var teste = scope.ServiceProvider.GetRequiredService<Teste>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                logger.LogInformation("Iniciando aplicação");

                teste.ConverterParaInt("1", false);
                teste.ConverterParaInt("aaaa", false);
                try
                {
                    teste.ConverterParaInt("bbbb", true);
                }
                catch { }

                logger.LogTrace("Exemplo Trace");
                logger.LogDebug("Exemplo Debug");
                logger.LogInformation("Exemplo Info");
                logger.LogWarning("Exemplo Warn");
                logger.LogError("Exemplo Error");
                logger.LogCritical("Exemplo Fatal");
                nlogger.Info("Exemplo log acessando diretamente NLog");
            }
            catch (Exception e)
            {
                nlogger.Error(e, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Aguardar flush de todos os logs para o targets
                LogManager.Shutdown();
            }
        }

        private static IServiceProvider BuildDi(IConfiguration config)
        {
            return new ServiceCollection()
               .AddScoped<Teste>() // Runner is the custom class
               .AddLogging(loggingBuilder =>
               {
                   // configure Logging with NLog
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog(config);
               })
               .BuildServiceProvider();
        }
    }
}
