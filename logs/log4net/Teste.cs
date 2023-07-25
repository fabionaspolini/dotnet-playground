using log4net;
using System;

namespace Log4NetPlayground
{
    class Teste
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int ConverterParaInt(string value, bool fatal)
        {
            Logger.Debug($"Iniciando conversão. value: {value} | fatal: {fatal}");
            try
            {
                Logger.Info($"Convertendo {value} para int");
                return int.Parse(value);
            }
            catch (Exception e)
            {
                // Usar "Error" quando a exception é capturada mas o processo segue, "Fatal" quando a thread é abortada
                if (fatal)
                {
                    Logger.Fatal($"Erro ao converter {value} para int", e);
                    throw;
                }
                else
                {
                    Logger.Error($"Erro ao converter {value} para int", e);
                    return 0;
                }
            }
            finally
            {
                Logger.Debug("Finalizando");
            }
        }
    }
}
