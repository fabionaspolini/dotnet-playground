using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Json;
using static System.Console;

namespace microsoft_logging_to_serilog_playground;

class Program
{
    static void Main(string[] args)
    {
        WriteLine(".:: Serilog como proxy para Microsoft Logging ::.");
        Trace.CorrelationManager.ActivityId = Guid.NewGuid();

        const string txtLogTemplate = "{Timestamp:dd/MM/yyyy HH:mm:ss.fff} {Level:u3} [{ActivityId}] [{SourceContext}] {Message:lj}{NewLine}{Exception}";
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console(
                outputTemplate: txtLogTemplate)
            .WriteTo.File(
                path: @".\Logs\SerilogAsMicrosoftProxy-Sample.log",
                outputTemplate: txtLogTemplate,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 10,
                encoding: Encoding.UTF8)
            .WriteTo.File(
                path: @".\Logs\SerilogAsMicrosoftProxy-Sample.json.log",
                formatter: new JsonFormatter(renderMessage: true),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 10,
                encoding: Encoding.UTF8)
            .Enrich.With(new ActivityIdEnricher())
            .Enrich.WithCorrelationId()
            .Enrich.FromLogContext()
            .CreateLogger();

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
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddSerilog(dispose: true);
            })
            .BuildServiceProvider();
    }
}