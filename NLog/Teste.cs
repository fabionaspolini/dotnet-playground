using System;
using System.Threading;

namespace NLog_Sample
{
    class Teste
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public int ConverterParaInt(string value)
        {
            Logger.Trace("Iniciando");
            try
            {
                Logger.Info($"Convertendo {value} para int");
                return int.Parse(value);
            }
            catch (Exception e)
            {
                // Usar "Error" quando a exception é capturada mas o processo segue, "Fatal" quando a thread é abortada
                Logger.Fatal(e, $"Erro ao converter {value} para int");
                return 0;
            }
            finally
            {
                Logger.Trace("Finalizando");
            }
        }
    }
}
