using System;
using System.Diagnostics;
using NLog;
using static System.Console;

namespace nlog_playground;

class Program
{
    // Neste exemplo é recuperado a instância do gerenciador de logs através da classe LogManager
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    static void Main(string[] args)
    {
        WriteLine(".:: NLog ::.");
        Trace.CorrelationManager.ActivityId = Guid.NewGuid();
        Logger.Info("Iniciando aplicação");

        new Teste().ConverterParaInt("1", false);
        new Teste().ConverterParaInt("aaaa", false);
        try
        {
            new Teste().ConverterParaInt("bbbb", true);
        }
        catch { }

        var logEvent = new LogEventInfo(LogLevel.Debug, null, "Exemplo de log com informações adicionais");
        logEvent.Properties["MinhaPropriedade"] = "ABC";
        logEvent.Properties["MinhaPropriedade2"] = "DEF";
        logEvent.Properties["MinhaPropriedade3"] = new { Id = 1, Nome = "Teste" };
        Logger.Log(logEvent);

        Logger.Debug("Exemplo de outro log estruturado {nome} {idade}.", "Exemplo", 25); // Exemplo de log estruturado montando objeto { nome = "Exemplo", idade = 25 }

        Logger.Trace("Exemplo Trace");
        Logger.Debug("Exemplo Debug");
        Logger.Info("Exemplo Info");
        Logger.Warn("Exemplo Warn");
        Logger.Error("Exemplo Error");
        Logger.Fatal("Exemplo Fatal");

        using (Logger.PushScopeNested("Escopo teste"))
        {
            Logger.Info("Log nivel 1");
            using (Logger.PushScopeNested("Subescopo teste"))
            {
                Logger.Info("Log nivel 2. Nome: {nome}, Idade: {idade}", "Teste", 30);
            }
        }

        // Aguardar flush de todos os logs para o targets
        LogManager.Shutdown();
    }
}