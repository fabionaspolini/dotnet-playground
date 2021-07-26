using System.Threading;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LangFeatures_Sample
{
    [PrimaryConstructor]
    public partial class InMemoryQueue
    {
        private readonly ILogger<InMemoryQueue> _logger;
        private ConcurrentQueue<string> _concurrentQueue = new();
        // private BlockingCollection<string> _concurrentQueue = new();

        public void TestQueue()
        {
            var messageQueue = new Queue<string>();
            messageQueue.Enqueue("Hello");
            messageQueue.Enqueue("World!");

            _logger.LogInformation(messageQueue.Dequeue());
            _logger.LogInformation(messageQueue.Dequeue());
        }

        public void TestConcurrentQueue()
        {
            var worker = StartConcurrentWorker();
            //foreach (var i in Enumerable.Range(1, 10))

            var i = 1;
            while (i <= 50)
            {
                if (_concurrentQueue.Count >= 5)
                {
                    _logger.LogInformation($"Sleeping...");
                    Task.Delay(TimeSpan.FromSeconds(1));
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
                else
                {
                    _logger.LogInformation($"Enqueuing item {i}");
                    _concurrentQueue.Enqueue(i.ToString());
                    Thread.Sleep(TimeSpan.FromMilliseconds(500));
                    i++;
                }
            }
        }

        private Task StartConcurrentWorker()
        {
            return Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    while (_concurrentQueue.TryDequeue(out var item))
                        _logger.LogInformation($"Dequeued item: {item}");

                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            });
        }
    }
}