using NLog;
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
            Logger.Info("Iniciando aplicação");

            new Teste().ConverterParaInt("1");
            new Teste().ConverterParaInt("aaaa");

            LogEventInfo logEvent = new LogEventInfo(LogLevel.Debug, null, "Exemplo de log com informações adicionais");
            logEvent.Properties["MinhaPropriedade"] = "ABC";
            logEvent.Properties["MinhaPropriedade2"] = "DEF";
            logEvent.Properties["MinhaPropriedade3"] = new { Id = 1, Nome = "Teste" };
            Logger.Log(logEvent);

            // Aguardar flush de todos os logs para o targets
            NLog.LogManager.Shutdown();
        }
    }
}
