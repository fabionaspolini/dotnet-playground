using System;
using Microsoft.Extensions.Logging;

namespace LanguageFeatures_Sample
{
    public class Exceptions
    {
        private ILogger<Exceptions> _logger;

        public Exceptions(ILogger<Exceptions> logger)
        {
            _logger = logger;
        }

        public void Execute()
        {
            try
            {
                int.Parse("a");
            }
            catch (FormatException e)
            {
                int.Parse("b");
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Erro inesperado");
            }
        }
    }
}
