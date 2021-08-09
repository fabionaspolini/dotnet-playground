using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LangFeatures_Sample
{
    [PrimaryConstructor]
    public partial class Principal : IHostedService
    {
        private readonly IHost _host;
        private readonly StreamForEach _streamForEach;
        private readonly ILogger<Principal> _logger;
        private readonly InMemoryQueue _inMemoryQueue;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(".:: Language Features Samples ::.");

            // await _streamForEach.ExecuteAsync();
            //_streamForEach.Execute();
            // _inMemoryQueue.TestQueue();
            //_inMemoryQueue.TestConcurrentQueue();

            // Tuplas();
            ApplicationPath.Execute();

            await Task.Delay(1000); // Tempo para flush dos logs no console
            _logger.LogInformation("Fim");
            // await _host.StopAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private void Tuplas()
        {
            var dic = new Dictionary<(string topic, string queue), int>();
            dic.Add((topic: "teste", queue: "1"), 1);
            dic.Add((topic: "teste", queue: "2"), 2);
            dic.Add((topic: "teste", queue: "3"), 3);
            dic.Add((topic: "teste2", queue: "1"), 21);
            dic.Add((topic: "teste2", queue: "2"), 22);
            dic.Add((topic: "teste2", queue: "3"), 23);

            if (dic.TryGetValue((topic: "teste", queue: "2"), out var teste))
                Console.WriteLine("1. Existe");

            if (!dic.TryGetValue((topic: "teste2", queue: "155"), out var teste2))
                Console.WriteLine("2. Não existe");

            Console.WriteLine($"Teste: {teste}");
            Console.WriteLine($"Teste2: {teste2}");
        }
    }
}
