using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using static System.Console;

namespace SerilogAsMicrosoftProxy_Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine(".:: Serilog como proxy para Microsoft Logging ::.");
            
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            
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


                logger.LogDebug("Exemplo de outro log estruturado {nome} {idade}.", "Exemplo", 25); // Exemplo de log estruturado montando objeto { nome = "Exemplo", idade = 25 }
                logger.LogTrace("Exemplo Trace");
                logger.LogDebug("Exemplo Debug");
                logger.LogInformation("Exemplo Info");
                logger.LogWarning("Exemplo Warn");
                logger.LogError("Exemplo Error");
                logger.LogCritical("Exemplo Fatal");
                Log.Information("Exemplo log acessando diretamente Serilog");

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
                Log.Error(e, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Aguardar flush de todos os logs para o targets
                Log.CloseAndFlush();
            }
        }

        private static IServiceProvider BuildDi(IConfiguration config)
        {
            return new ServiceCollection()
               .AddScoped<Teste>() // Runner is the custom class
               .AddLogging(loggingBuilder =>
               {
                   // configure Logging with Serilog
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddSerilog(dispose: true);
               })
               .BuildServiceProvider();
        }
    }
}
