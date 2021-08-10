using Serilog;
using System;
using System.Threading;

namespace Serilog_Sample
{
    class Teste
    {
        public int ConverterParaInt(string value, bool fatal)
        {
            Log.Verbose($"Iniciando conversão. value: {value} | fatal: {fatal}");
            try
            {
                Log.Information($"Convertendo {value} para int");
                return int.Parse(value);
            }
            catch (Exception e)
            {
                // Usar "Error" quando a exception é capturada mas o processo segue, "Fatal" quando a thread é abortada
                if (fatal)
                {
                    Log.Fatal(e, $"Erro ao converter {value} para int");
                    throw;
                }
                else
                {
                    Log.Error(e, $"Erro ao converter {value} para int");
                    return 0;
                }
            }
            finally
            {
                Log.Verbose("Finalizando");
            }
        }
    }
}
