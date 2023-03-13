using Serilog;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Serilog.Formatting.Json;
using static System.Console;

namespace Serilog_Sample;

class Program
{
    static void Main(string[] args)
    {
        WriteLine(".:: Serilog ::.");
        Trace.CorrelationManager.ActivityId = Guid.NewGuid();
        
        const string txtLogTemplate = "{Timestamp:dd/MM/yyyy HH:mm:ss.fff} {Level:u3} [{ActivityId}] [{SourceContext}] {Message:lj}{NewLine}{Exception}";
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console(
                outputTemplate: txtLogTemplate)
            .WriteTo.File(
                path: @".\Logs\Serilog-Sample.log",
                outputTemplate: txtLogTemplate,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 10,
                encoding: Encoding.UTF8)
            .WriteTo.File(
                path: @".\Logs\Serilog-Sample.json.log",
                formatter: new JsonFormatter(renderMessage: true),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 10,
                encoding: Encoding.UTF8)
            .Enrich.With(new ActivityIdEnricher())
            .Enrich.WithCorrelationId()
            .Enrich.FromLogContext()
            .CreateLogger();

        Log.Information("Iniciando aplicação");

        new Teste().ConverterParaInt("1", false);
        new Teste().ConverterParaInt("aaaa", false);
        try
        {
            new Teste().ConverterParaInt("bbbb", true);
        }
        catch { }

        Log.Debug("Exemplo de log com informações adicionais {nome} {user}", "ABC", new { Id = 1, Nome = "Teste" });

        Log.Logger.ForContext<Program>().Information("Exemplo de log salvando nome do método executor");

        Log.Verbose("Exemplo Trace");
        Log.Debug("Exemplo Debug");
        Log.Information("Exemplo Info");
        Log.Warning("Exemplo Warn");
        Log.Error("Exemplo Error");
        Log.Fatal("Exemplo Fatal");

        // Aguardar flush de todos os logs para o targets
        Log.CloseAndFlush();
    }
}

public static class LoggerExtensions
{
    public static ILogger Here(this ILogger logger,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        return logger
            .ForContext("MemberName", memberName)
            .ForContext("FilePath", sourceFilePath)
            .ForContext("LineNumber", sourceLineNumber);
    }
}