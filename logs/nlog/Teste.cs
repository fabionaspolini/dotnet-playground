using System;
using System.Threading;

namespace NLog_Sample
{
    class Teste
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public int ConverterParaInt(string value, bool fatal)
        {
            Logger.Trace($"Iniciando conversão. value: {value} | fatal: {fatal}"); // Log não estruturado => String concatenada
            try
            {
                Logger.Info("Convertendo {value} para int", value); // Log estruturado => O framework faz replace de {value} e monta objeto na memória { value = "valor do argumento" }
                return int.Parse(value);
            }
            catch (Exception e)
            {
                // Usar "Error" quando a exception é capturada mas o processo segue, "Fatal" quando a thread é abortada
                if (fatal)
                {
                    Logger.Fatal(e, "Erro ao converter {value} para int", value);
                    throw;
                }
                else
                {
                    Logger.Error(e, "Erro ao converter {value} para int", value);
                    return 0;
                }
            }
            finally
            {
                Logger.Trace("Finalizando");
            }
        }
    }
}
