using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Encodings.Web;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(".:: Microsoft Logging - Benchmark ::.");
        //Trace.CorrelationManager.ActivityId = Guid.NewGuid();

        //BenchmarkRunner.Run(typeof(Program).Assembly);

        //BenchmarkRunner.Run<ConsoleBenchmark>();
        //BenchmarkRunner.Run<SimpleConsoleBenchmark>();
        //BenchmarkRunner.Run<JsonBenchmark>();
        //BenchmarkRunner.Run<MyJsonBenchmark>();

        BenchmarkRunner.Run(new Type[] {
            typeof(ConsoleBenchmark),
            typeof(SimpleConsoleBenchmark),
            typeof(JsonBenchmark),
            typeof(MyJsonBenchmark),
        });
    }
}

// ShortRunJob / SimpleJob
[ShortRunJob(RuntimeMoniker.Net70), AllStatisticsColumn, RPlotExporter]
//[SimpleJob(RuntimeMoniker.Net70)]
//[RPlotExporter]
public class ConsoleBenchmark : LoggingBenchmarkBase<ConsoleBenchmark>
{
    protected override IServiceProvider CreateServiceProvider() => Startup.BuildServices(LoggerProvider.Console);
}

[ShortRunJob(RuntimeMoniker.Net70), AllStatisticsColumn, RPlotExporter]
public class SimpleConsoleBenchmark : LoggingBenchmarkBase<SimpleConsoleBenchmark>
{
    protected override IServiceProvider CreateServiceProvider() => Startup.BuildServices(LoggerProvider.SimpleConsole);
}

[ShortRunJob(RuntimeMoniker.Net70), AllStatisticsColumn, RPlotExporter]
public class JsonBenchmark : LoggingBenchmarkBase<JsonBenchmark>
{
    protected override IServiceProvider CreateServiceProvider() => Startup.BuildServices(LoggerProvider.JsonConsole);
}

[ShortRunJob(RuntimeMoniker.Net70), AllStatisticsColumn, RPlotExporter]
public class MyJsonBenchmark : LoggingBenchmarkBase<MyJsonBenchmark>
{
    protected override IServiceProvider CreateServiceProvider() => Startup.BuildServices(LoggerProvider.MyJsonConsole);
}

public abstract class LoggingBenchmarkBase<TLoggerCategory>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ILogger<TLoggerCategory> _logger;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    protected abstract IServiceProvider CreateServiceProvider();

    [GlobalSetup]
    public void Setup()
    {
        var services = CreateServiceProvider();
        _logger = services.GetRequiredService<ILogger<TLoggerCategory>>();
    }

    [Benchmark]
    public void SimpleLog() => _logger.LogInformation(
        "Nome: Fulano, Idade: 10, Cidade: São Paulo, Estado: SP, Pais: Brasil");

    [Benchmark]
    public void OneTemplateLog() => _logger.LogInformation(
        "Nome: {Nome}", "Fulano, Idade: 10, Cidade: São Paulo, Estado: SP, Pais: Brasil");

    [Benchmark]
    public void FiveTemplateLog() => _logger.LogInformation(
        "Nome: {Nome}, Idade: {Idade}, Cidade: {Cidade}, Estado: {Estado}, Pais: {Pais}",
        "Fulano", 10, "São Paulo", "SP", "Brasil");
}

public enum LoggerProvider
{
    Console,
    SimpleConsole,
    JsonConsole,
    MyJsonConsole
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

                if (loggerProvider == LoggerProvider.JsonConsole)
                    builder.AddJsonConsole(x =>
                    {
                        x.IncludeScopes = true;
                        x.JsonWriterOptions = new()
                        {
                            Indented = false, // Indentação causa muita queda de performance
                            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        };
                    });

                if (loggerProvider == LoggerProvider.MyJsonConsole)
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