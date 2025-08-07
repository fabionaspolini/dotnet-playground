using System;
using System.Diagnostics;
using log4net;
using static System.Console;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace log4net_playground;

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

        ThreadContext.Properties["MinhaPropriedade"] = "ABC";
        ThreadContext.Properties["MinhaPropriedade2"] = "DEF";
        ThreadContext.Properties["MinhaPropriedade3"] = new { Id = 1, Nome = "Teste" };
        Logger.Debug("Exemplo de log com informações adicionais");
        ThreadContext.Properties.Clear();

        // Logger.Debug("Exemplo de outro log estruturado {nome} {idade}.", "Exemplo", 25); // Não há suporte a este layout de log estruturado

        Logger.Debug("Não há suporte ao level Trace");
        Logger.Debug("Exemplo Debug");
        Logger.Info("Exemplo Info");
        Logger.Warn("Exemplo Warn");
        Logger.Error("Exemplo Error");
        Logger.Fatal("Exemplo Fatal");
    }
}