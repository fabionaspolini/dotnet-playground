using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace LangFeatures_Sample
{
    public class MyBackgroundService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"MyBackgroundService {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}