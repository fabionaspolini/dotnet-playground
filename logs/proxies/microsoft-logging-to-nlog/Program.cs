using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.Diagnostics;
using static System.Console;

namespace MicrosoftLoggingToNLogPlayground;

class Program
{
    static void Main(string[] args)
    {
        WriteLine(".:: NLog como proxy para Microsoft Logging ::.");

        Trace.CorrelationManager.ActivityId = Guid.NewGuid();

        var config = new ConfigurationBuilder().Build();
        var serviceProvider = BuildDi(config);

        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        try
        {
            var teste = scope.ServiceProvider.GetRequiredService<Teste>();

            logger.LogInformation("Iniciando aplicação");

            teste.ConverterParaInt("1", false);
            teste.ConverterParaInt("aaaa", false);
            try
            {
                teste.ConverterParaInt("bbbb", true);
            }
            catch { }


            logger.LogDebug("Exemplo de outro log estruturado {nome} {idade}", "Exemplo", 25); // Exemplo de log estruturado montando objeto { nome = "Exemplo", idade = 25 }
            logger.LogTrace("Exemplo Trace");
            logger.LogDebug("Exemplo Debug");
            logger.LogInformation("Exemplo Info");
            logger.LogWarning("Exemplo Warn");
            logger.LogError("Exemplo Error");
            logger.LogCritical("Exemplo Fatal");
            LogManager.GetCurrentClassLogger().Info("Comparativo de log acessando enviado diretamente para o NLog");

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
        catch (Exception e)
        {
            logger.LogError(e, "Stopped program because of exception");
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
                loggingBuilder.AddNLog(config, new NLogProviderOptions
                {
                    IncludeScopes = true
                });
            })
            .BuildServiceProvider();
    }
}