using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace NLogAsMicrosoftProxy_Sample
{
    class Teste
    {
        private readonly ILogger<Teste> _logger;

        public Teste(ILogger<Teste> logger)
        {
            _logger = logger;
        }

        public int ConverterParaInt(string value, bool fatal)
        {
            _logger.LogTrace($"Iniciando conversão. value: {value} | fatal: {fatal}");
            try
            {
                _logger.LogInformation($"Convertendo {value} para int");
                return int.Parse(value);
            }
            catch (Exception e)
            {
                // Usar "Error" quando a exception é capturada mas o processo segue, "Fatal" quando a thread é abortada
                if (fatal)
                {
                    _logger.LogCritical(e, $"Erro ao converter {value} para int");
                    throw;
                }
                else
                {
                    _logger.LogError(e, $"Erro ao converter {value} para int");
                    return 0;
                }
            }
            finally
            {
                _logger.LogTrace("Finalizando");
            }
        }
    }
}
