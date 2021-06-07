using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LangFeatures_Sample
{
    [PrimaryConstructor]
    public partial class Program : IHostedService
    {
        private readonly StreamForEach _streamForEach;
        private readonly ILogger<Program> _logger;
        private readonly IHost _host;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(".:: Language Features Samples ::.");

            await _streamForEach.ExecuteAsync();
            _streamForEach.Execute();

            _logger.LogInformation("Fim");
            await _host.StopAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
