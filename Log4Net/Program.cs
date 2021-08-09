using log4net;
using System;
using System.Diagnostics;
using static System.Console;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace Log4Net_Sample
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            WriteLine(".:: Log4Net ::.");
            Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            Logger.Info("Iniciando aplicação");

            new Teste().ConverterParaInt("1", false);
            new Teste().ConverterParaInt("aaaa", false);
            try
            {
                new Teste().ConverterParaInt("bbbb", true);
            }
            catch { }

            log4net.ThreadContext.Properties["MinhaPropriedade"] = "ABC";
            log4net.ThreadContext.Properties["MinhaPropriedade2"] = "DEF";
            log4net.ThreadContext.Properties["MinhaPropriedade3"] = new { Id = 1, Nome = "Teste" };
            Logger.Debug("Exemplo de log com informações adicionais");
            log4net.ThreadContext.Properties.Clear();

            Logger.Debug("Não há suporte ao level Trace");
            Logger.Debug("Exemplo Debug");
            Logger.Info("Exemplo Info");
            Logger.Warn("Exemplo Warn");
            Logger.Error("Exemplo Error");
            Logger.Fatal("Exemplo Fatal");
        }
    }
}
