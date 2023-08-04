using Dapper;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

Console.WriteLine(".:: SQL Server Playground - Benchmark ::.");

// Calibração do teste
TimeSpan TotalTestTime = TimeSpan.FromSeconds(10);
const int Threads = 1;
var appName = AppDomain.CurrentDomain.FriendlyName;
// --

Console.WriteLine("Iniciando tasks...");
var tasks = new List<Task<int>>();
foreach (var i in Enumerable.Range(1, Threads))
    tasks.Add(StartWorkerTask(i));

Task.WaitAll(tasks.ToArray());
var totalOperations = tasks.Sum(x => x.Result);

Console.WriteLine();
Console.WriteLine($"Total: {totalOperations:N0} - {totalOperations / TotalTestTime.TotalSeconds:N1} op/sec");
Console.WriteLine($"Tempo: {TotalTestTime}");

Console.WriteLine();
Console.WriteLine("Fim");

Task<int> StartWorkerTask(int taskId) => Task.Run(async () =>
{
    // Teste sem multiplos comandos por conexão
    var conn = new SqlConnection($"Server=127.0.0.1;Database=teste;User Id=sa;Password=Pass123456;Application Name={appName};TrustServerCertificate=True;");
    await conn.OpenAsync();
    var warmUpResult = await conn.QueryFirstAsync<Pessoa>("select * from pessoa where id = @id", new { id = 1 }); // Aquecer para libraries serem carregas para memória

    var watch = Stopwatch.StartNew();
    var count = 0;
    while (watch.Elapsed < TotalTestTime)
    {
        var result = await conn.QueryFirstAsync<Pessoa>("select * from pessoa where id = @id", new { id = 1 });
        count++;
        if (count % 1_000 == 0)
            Console.WriteLine($"Task {taskId}: {count:N0} - {watch.Elapsed}");
    }
    watch.Stop();
    Console.WriteLine($"Task {taskId}: {count:N0} - {watch.Elapsed} - {count / watch.Elapsed.TotalSeconds:N1} op/sec - Fim");
    return count;
});

record Pessoa(int Id, string Nome);