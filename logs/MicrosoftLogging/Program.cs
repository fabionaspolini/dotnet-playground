using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MicrosoftLogging_Sample.MyLogger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Console;

namespace MicrosoftLogging_Sample
{
    record class ServiceOptions(string Url, string AccessKey);

    class Program
    {
        static void Main(string[] args)
        {
            WriteLine(".:: Microsoft Logging ::.");

            Trace.CorrelationManager.ActivityId = Guid.NewGuid();

            var config = BuildConfig();
            var serviceProvider = BuildServices(config);

            using var scope = serviceProvider.CreateScope();

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var teste = scope.ServiceProvider.GetRequiredService<Teste>();

            logger.LogInformation("Iniciando aplicação");

            logger.LogTrace("Exemplo Trace");
            logger.LogDebug("Exemplo Debug");
            logger.LogInformation("Exemplo Info");
            logger.LogWarning("Exemplo Warn");
            logger.LogError("Exemplo Error");
            logger.LogCritical("Exemplo Fatal");
            logger.LogDebug("Exemplo de outro log estruturado {nome} {idade}.", "Exemplo", 25); // Microsoft Logging não gera informação adicional para log estruturado

            //using (logger.BeginScope("TransactionId: {TransactionId}", Guid.NewGuid()))
            using (logger.BeginScope(new Dictionary<string, object> { { "TransactionId", Guid.NewGuid() } })) // Com MyLogger fica bem formatado
            {
                logger.LogInformation("Teste");

                using (logger.BeginScope(new Dictionary<string, object> { { "Teste", Guid.NewGuid() } }))
                {
                    logger.LogInformation("Teste 2");
                }
            }

            teste.ConverterParaInt("1", false);
            teste.ConverterParaInt("aaaa", false);
            try
            {
                teste.ConverterParaInt("bbbb", true);
            }
            catch { }

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

        private static IConfigurationRoot BuildConfig() => new ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        private static IServiceProvider BuildServices(IConfigurationRoot config)
        {
            return new ServiceCollection()
                .AddSingleton(config)
               .AddScoped<Teste>() // Runner is the custom class
               .AddLogging(builder =>
               {
                   builder.ClearProviders();
                   builder.AddConfiguration(config.GetSection("Logging"));
                   /*builder.AddConsole(options =>
                   {
                       options.IncludeScopes = true;
                       options.TimestampFormat = "dd/MM/yyyy HH:mm:ss.fff ";
                   });*/
                   /*builder.AddSimpleConsole(options =>
                   {
                       options.IncludeScopes = true;
                       options.SingleLine = true;
                       options.TimestampFormat = "dd/MM/yyyy HH:mm:ss.fff ";
                   });*/
                   /*builder.AddJsonConsole(x =>
                   {
                       x.IncludeScopes = true;
                       x.JsonWriterOptions = new() { Indented = true };
                   });*/
                   builder.AddMyLogger();
               })
               .BuildServiceProvider();
        }
    }
}
