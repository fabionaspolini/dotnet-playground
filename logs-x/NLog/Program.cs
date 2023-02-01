using NLog;
using System;
using System.Diagnostics;
using static System.Console;

namespace NLog_Sample
{
    class Program
    {
        // Neste exemplo é recuperado a instância do gerenciador de logs através da classe LogManager
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
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

            LogEventInfo logEvent = new LogEventInfo(LogLevel.Debug, null, "Exemplo de log com informações adicionais");
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

            // Aguardar flush de todos os logs para o targets
            NLog.LogManager.Shutdown();
        }
    }
}
