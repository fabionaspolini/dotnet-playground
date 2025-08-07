﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace miscellaneous_features_playground;

[PrimaryConstructor]
public partial class Principal : IHostedService
{
    private readonly IHost _host;
    // private readonly StreamForEach _streamForEach;
    private readonly ILogger<Principal> _logger;
    private readonly InMemoryQueue _inMemoryQueue;
    // private readonly Exceptions _exceptions;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation(".:: language-features Samples ::.");

        // await _streamForEach.ExecuteAsync();
        //_streamForEach.Execute();
        // _inMemoryQueue.TestQueue();
        _inMemoryQueue.TestConcurrentQueue();

        // Tuplas.Execute();
        // ApplicationPath.Execute();
        // _exceptions.Execute();
        // Immutable.Tests.Execute();

        await Task.Delay(1000); // Tempo para flush dos logs no console
        _logger.LogInformation("Fim");
        // await _host.StopAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}