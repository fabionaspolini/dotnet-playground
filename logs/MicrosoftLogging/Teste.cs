﻿using Microsoft.Extensions.Logging;
using System;

namespace MicrosoftLogging_Sample;

class Teste
{
    private readonly ILogger<Teste> _logger;

    public Teste(ILogger<Teste> logger)
    {
        _logger = logger;
    }

    public int ConverterParaInt(string value, bool fatal)
    {
        using (_logger.BeginScope("Convertendo {Value} para inteiro (fatal={Fatal})", value, fatal))
        {
            _logger.LogTrace($"Iniciando conversão");
            try
            {
                return int.Parse(value);
            }
            catch (Exception e)
            {
                // Usar "Error" quando a exception é capturada mas o processo segue, "Fatal" quando a thread é abortada
                if (fatal)
                {
                    _logger.LogCritical(e, $"Erro na conversão");
                    throw;
                }
                else
                {
                    _logger.LogError(e, $"Erro na conversão");
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