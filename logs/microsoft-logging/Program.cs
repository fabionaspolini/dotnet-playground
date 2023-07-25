using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Threading;
using static System.Console;

namespace MicrosoftLoggingPlayground
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
            logger.LogDebug("Exemplo de outro log estruturado {Nome} {Idade}.", "Exemplo", 25); // Microsoft Logging não gera informação adicional para log estruturado
            logger.LogInformation(10, "Teste com EventId");

            //using (logger.BeginScope("TransactionId: {TransactionId}", Guid.NewGuid()))
            using (logger.BeginScope(new Dictionary<string, object> { { "TransactionId", Guid.NewGuid() } })) // Com MyLogger fica bem formatado
            {
                logger.LogInformation("Teste");

                using (logger.BeginScope(new Dictionary<string, object> { { "Teste", Guid.NewGuid() } }))
                {
                    logger.LogInformation("Teste 2");
                    logger.LogInformation("Teste com variável {Nome}", "Fulano");
                    using (logger.BeginScope("Scope apenas texto"))
                        logger.LogInformation("Teste com scope apenas texto");
                }

                logger.LogInformation("Teste 3");
            }

            teste.ConverterParaInt("1", false);
            teste.ConverterParaInt("aaaa", false);
            try
            {
                teste.ConverterParaInt("bbbb", true);
            }
            catch { }

            using (logger.BeginScope("Exemplo de escopo (nome={Nome}, idade={Idade})", "Exemplo", 25))
            {
                try
                {

                    logger.LogInformation("Teste");
                    logger.LogInformation("Teste 2");
                    using (logger.BeginScope("Exemplo de subescopo (endereco={Endereco}, cidade={Cidade})", "Rua exemplo", "São Paulo"))
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

            // Gerar TraceId com new Activity("....").
            // Cada activity gerada possui SpanId diferente, mas o mesmo TraceId
            // Não é preservado ordem entre BeginScopes() e new Activity()
            using (logger.BeginScope("Iniciando scope antes da acivity"))
            {
                var activity = new Activity("Atividade de início do processo");
                activity.Start();
                //activity.ActivityTraceFlags = ActivityTraceFlags.Recorded;
                activity.AddBaggage("Baggage Act 1", Guid.NewGuid().ToString()); // Adiciona uma mensagem no scope de log como string. Child Activities levam esse dado.
                activity.AddTag("Tag Act 1", Guid.NewGuid().ToString()); // Adiciona um elemento no scope de log como se fosse um logger.BeginScope(). Logger padrão imprime apenas as tags da Activity corrente.
                activity.SetCustomProperty("custom property", "aaaaaaaaa"); // Não loga no console.
                                                                            //activity.TraceStateString = "Teste"; // Compartilha no trace distribuido
                logger.LogInformation("Iniciando processo");
                Thread.Sleep(1000);

                using (logger.BeginScope("Iniciando scope dentro do sub-processo: ScopeSubProcessId: {ScopeSubProcessId}", Guid.NewGuid()))
                {
                    logger.LogInformation("Scope message - 1");

                    var activity2 = new Activity("Atividade para sub-processo");
                    activity2.Start();
                    activity2.AddBaggage("Baggage Act 2", "teste"); // Irá concatenar com o dado da primeira activity
                    activity2.AddTag("Tag Act 2", "teste");
                    logger.LogInformation("Executando sub-processo"); // Console logger padrão imprime apenas tag da Activity 2. O customizado imprime todos.


                    activity2.Stop();
                    logger.LogInformation("Activity 2 stoped");
                }

                activity.Stop();
                logger.LogInformation("Activity 1 stoped");
            }

            logger.LogInformation("Fim");
        }

        private static IConfigurationRoot BuildConfig() => new ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        private static IServiceProvider BuildServices(IConfigurationRoot config) => new ServiceCollection()
                .AddSingleton(config)
                .AddScoped<Teste>() // Runner is the custom class
                .AddLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddConfiguration(config.GetSection("Logging"));

                    // Adicionar scope no log com o traceId gerado com new Activity("...")
                    builder.Configure(x => x.ActivityTrackingOptions = ActivityTrackingOptions.SpanId |
                        ActivityTrackingOptions.TraceId |
                        ActivityTrackingOptions.ParentId |
                        ActivityTrackingOptions.Tags |
                        ActivityTrackingOptions.Baggage);

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
                        x.JsonWriterOptions = new() { Indented = true }; // Indentação causa muita queda de performance
                    });*/
                    //builder.AddMyLogger();
                    builder.AddMyJsonFormatterConsole(x =>
                    {
                        x.IncludeScopes = true;
                        x.TimestampFormat = "O";
                        x.JsonWriterOptions = new()
                        {
                            Indented = true, // Indentação causa muita queda de performance
                            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        };
                    });
                })
               .BuildServiceProvider();
    }
}
