using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using static System.Console;

namespace MicrosoftLogging_Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine(".:: Microsoft Logging ::.");

            Trace.CorrelationManager.ActivityId = Guid.NewGuid();

            var config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
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

            logger.LogDebug("Exemplo de outro log estruturado {nome} {idade}.", "Exemplo", 25); // Microsoft Logging não gera informação adicional para log estruturado
            logger.LogTrace("Exemplo Trace");
            logger.LogDebug("Exemplo Debug");
            logger.LogInformation("Exemplo Info");
            logger.LogWarning("Exemplo Warn");
            logger.LogError("Exemplo Error");
            logger.LogCritical("Exemplo Fatal");

            using (logger.BeginScope("Exemplo de escopo (nome={nome}, idade={idade})", "Exemplo", 25))
            {
                try
                {

                    logger.LogInformation("Teste");
                    logger.LogInformation("Teste 2");
                    using (logger.BeginScope("Exemplo de subescopo (endereco={endereco}, cidade={cidade})", "Rua exemplo", "São Paulo"))
                    {
                        logger.LogInformation("Teste 3");
                        logger.LogInformation("Teste 4");
                        throw new Exception("Erro exemplo");
                    }
                }
                catch (Exception e)
                {
                    logger.LogCritical(e, "Erro ao realizar ....");
                }
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
                   /*loggingBuilder.AddConsole(options =>
                   {
                       options.IncludeScopes = true;
                       options.TimestampFormat = "dd/MM/yyyy HH:mm:ss.fff ";
                   });*/
                   /*loggingBuilder.AddSimpleConsole(options =>
                   {
                       options.IncludeScopes = true;
                       options.SingleLine = true;
                       options.TimestampFormat = "dd/MM/yyyy HH:mm:ss.fff ";
                   });*/
                   loggingBuilder.AddJsonConsole(x =>
                   {
                       x.IncludeScopes = true;
                   });
               })
               .BuildServiceProvider();
        }
    }
}
