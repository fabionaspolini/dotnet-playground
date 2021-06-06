using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FluentDate;
using Microsoft.Extensions.Logging;

namespace LangFeatures_Sample
{
    public class StreamForEach
    {
        private ILogger<StreamForEach> _logger;

        public StreamForEach(ILogger<StreamForEach> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Recurso C# 8 - Este método permite usar num loop "await foreach" uma função "async". A cada "yield return" é processado o "_logger.LogInformation".
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsync()
        {
            var time = new Stopwatch();
            time.Start();
            await foreach (var item in GetValuesFromStreamAsync())
            {
                _logger.LogInformation($"Stream: {item}");
                // await Task.Delay(2.Seconds());
            }
            time.Stop();
            _logger.LogInformation($"Total: {time.Elapsed}");
        }

        /// <summary>
        /// Este é o método tradicional, a cada "yield return" é processado o "_logger.LogInformation".
        /// </summary>
        public void Execute()
        {
            var time = new Stopwatch();
            time.Start();
            var values = GetValuesRetornoPadrao();
            foreach (var item in values)
            {
                _logger.LogInformation($"Padrão: {item}");
                // Thread.Sleep(500.Milliseconds());
            }
            time.Stop();
            _logger.LogInformation($"Total: {time.Elapsed}");
        }

        private async IAsyncEnumerable<string> GetValuesFromStreamAsync()
        {
            for (int i = 1; i <= 5; i++)
            {
                yield return $"Retorno {i}";
                await Task.Delay(1.Seconds());
            }
        }

        private IEnumerable<string> GetValuesRetornoPadrao()
        {
            for (int i = 1; i <= 5; i++)
            {
                yield return $"Retorno {i}";
                Thread.Sleep(1.Seconds());
            }
        }
    }
}
