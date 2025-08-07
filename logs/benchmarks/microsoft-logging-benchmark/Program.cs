using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Encodings.Web;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using microsoft_logging_web_api_playground;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace microsoft_logging_benchmark_playground;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(".:: Microsoft Logging - Benchmark ::.");
        //Trace.CorrelationManager.ActivityId = Guid.NewGuid();

        BenchmarkRunner.Run<LoggingBenchmark>();
    }
}


// ShortRunJob / SimpleJob
[ShortRunJob(RuntimeMoniker.Net70), AllStatisticsColumn, RPlotExporter]
//[SimpleJob(RuntimeMoniker.Net70), AllStatisticsColumn, RPlotExporter]
//[RPlotExporter]
public class LoggingBenchmark
{
    private static readonly Dictionary<string, object> _scopeInformationValues = new()
    {
        { "Empresa", "Teste" },
        { "Filial", "Matriz" },
        { "Cnpj", "000000000000000" }
    };

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ILogger<LoggingBenchmark> _logger;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [Params(LoggerProvider.MyJson)]
    //[Params(LoggerProvider.Console, LoggerProvider.SimpleConsole, LoggerProvider.Json, LoggerProvider.MyJson)]
    public LoggerProvider LoggerProvider;

    [Params(false, true)]
    public bool Scopes;

    [Params(false, true)]
    public bool Activity;

    private static readonly Action<ILogger, string, int, string, string, string, Exception?> _fiveTemaplteLogHighPerformanceLog = LoggerMessage.Define<string, int, string, string, string>(
        LogLevel.Information,
        new EventId(1, nameof(LoggingBenchmark)),
        "Nome: {Nome}, Idade: {Idade}, Cidade: {Cidade}, Estado: {Estado}, Pais: {Pais}");

    [GlobalSetup]
    public void Setup()
    {
        var services = Startup.BuildServices(LoggerProvider);
        _logger = services.GetRequiredService<ILogger<LoggingBenchmark>>();

    }

    [Benchmark]
    public void SimpleLog()
    {
        var activity = Activity ? StartActivity() : null;
        var scope = Scopes ? _logger.BeginScope(_scopeInformationValues) : null;

        _logger.LogInformation("Nome: Fulano, Idade: 10, Cidade: Sao Paulo, Estado: SP, Pais: Brasil");

        DisposeScope(ref scope);
        DisposeActivity(ref activity);
    }

    [Benchmark]
    public void OneTemplateLog()
    {
        var activity = Activity ? StartActivity() : null;
        var scope = Scopes ? _logger.BeginScope(_scopeInformationValues) : null;

        _logger.LogInformation("Nome: {Nome}", "Fulano, Idade: 10, Cidade: Sao Paulo, Estado: SP, Pais: Brasil");

        DisposeScope(ref scope);
        DisposeActivity(ref activity);
    }

    [Benchmark]
    public void FiveTemplateLog()
    {
        var activity = Activity ? StartActivity() : null;
        var scope = Scopes ? _logger.BeginScope(_scopeInformationValues) : null;

        _logger.LogInformation("Nome: {Nome}, Idade: {Idade}, Cidade: {Cidade}, Estado: {Estado}, Pais: {Pais}",
            "Fulano", 10, "Sao Paulo", "SP", "Brasil");

        DisposeScope(ref scope);
        DisposeActivity(ref activity);
    }

    //[Benchmark]
    public void FiveTemplateLogHighPerf()
    {
        var activity = Activity ? StartActivity() : null;
        var scope = Scopes ? _logger.BeginScope(_scopeInformationValues) : null;

        _fiveTemaplteLogHighPerformanceLog(_logger, "Fulano", 10, "Sao Paulo", "SP", "Brasil", null);

        DisposeScope(ref scope);
        DisposeActivity(ref activity);
    }

    private void DisposeScope(ref IDisposable? scope) => scope?.Dispose();

    private Activity StartActivity()
    {
        var result = new Activity("Operação de testes");
        result.Start();
        result.AddTag("Act tag 1", "Teste");
        result.AddTag("Act tag 2", 999);
        return result;
    }

    private void DisposeActivity(ref Activity? activity)
    {
        if (activity != null)
        {
            activity.Stop();
            activity.Dispose();
        }
    }
}

public enum LoggerProvider
{
    Console,
    SimpleConsole,
    Json,
    MyJson
}

public static class Startup
{
    public static IConfigurationRoot Configuration { get; }

    static Startup()
    {
        Configuration = BuildConfig();
    }

    private static IConfigurationRoot BuildConfig() => new ConfigurationBuilder()
        .SetBasePath(System.IO.Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();

    public static IServiceProvider BuildServices(LoggerProvider loggerProvider)
        => new ServiceCollection()
            .AddSingleton(Configuration)
            .AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConfiguration(Configuration.GetSection("Logging"));
                builder.SetMinimumLevel(LogLevel.Information);

                // Adicionar scope no log com o traceId gerado com new Activity("...")
                builder.Configure(x => x.ActivityTrackingOptions = ActivityTrackingOptions.SpanId |
                                                                   ActivityTrackingOptions.TraceId |
                                                                   ActivityTrackingOptions.ParentId |
                                                                   ActivityTrackingOptions.Tags |
                                                                   ActivityTrackingOptions.Baggage);
                //builder.Configure(x => x.ActivityTrackingOptions = ActivityTrackingOptions.None);

                if (loggerProvider == LoggerProvider.Console)
                    builder.AddConsole(options =>
                    {
                        options.IncludeScopes = true;
                        options.TimestampFormat = "dd/MM/yyyy HH:mm:ss.fff ";
                    });

                if (loggerProvider == LoggerProvider.SimpleConsole)
                    builder.AddSimpleConsole(options =>
                    {
                        options.IncludeScopes = true;
                        options.SingleLine = true;
                        options.TimestampFormat = "dd/MM/yyyy HH:mm:ss.fff ";
                    });

                if (loggerProvider == LoggerProvider.Json)
                    builder.AddJsonConsole(x =>
                    {
                        x.IncludeScopes = true;
                        x.JsonWriterOptions = new()
                        {
                            Indented = false, // Indentação causa muita queda de performance
                            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        };
                    });

                if (loggerProvider == LoggerProvider.MyJson)
                    builder.AddMyJsonFormatterConsole(x =>
                    {
                        x.IncludeScopes = true;
                        x.TimestampFormat = "O";
                        x.JsonWriterOptions = new()
                        {
                            Indented = false, // Indentação causa muita queda de performance
                            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        };
                    });
            })
            .BuildServiceProvider();
}