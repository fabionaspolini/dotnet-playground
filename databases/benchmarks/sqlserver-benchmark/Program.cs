using Dapper;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

Console.WriteLine(".:: SQL Server Playground - Benchmark ::.");

// Calibração do teste
const bool IsAsync = true;
const int Threads = 10;
var totalTestTime = TimeSpan.FromSeconds(10);
var appName = AppDomain.CurrentDomain.FriendlyName;
// --

Console.WriteLine("Iniciando tasks...");
var realTime = Stopwatch.StartNew();
var tasks = new List<Task<int>>();
foreach (var i in Enumerable.Range(1, Threads))
    tasks.Add(IsAsync ? StartWorkerTaskAsync(i) : StartWorkerTaskSync(i));

Task.WaitAll(tasks.ToArray());
realTime.Stop();
var totalOperations = tasks.Sum(x => x.Result);

Console.WriteLine();
Console.WriteLine($"IsAsync: {IsAsync}");
Console.WriteLine($"Total: {totalOperations:N0} - {totalOperations / totalTestTime.TotalSeconds:N1} op/sec");
Console.WriteLine($"Tempo: {totalTestTime} (real: {realTime.Elapsed})");

Console.WriteLine();
Console.WriteLine("Fim");

Task<int> StartWorkerTaskAsync(int taskId) => Task.Run(async () =>
{
    // Teste sem multiplos comandos por conexão
    await using var conn = new SqlConnection($"Server=127.0.0.1;Database=teste;User Id=sa;Password=Pass123456;Application Name={appName};TrustServerCertificate=True;");
    await conn.OpenAsync();
    var warmUpResult = await conn.QueryFirstAsync<Pessoa>("select * from pessoa where id = @id", new { id = 1 }); // Aquecer para libraries serem carregas para memória

    var watch = Stopwatch.StartNew();
    var count = 0;
    while (watch.Elapsed < totalTestTime)
    {
        var result = await conn.QueryFirstAsync<Pessoa>("select * from pessoa where id = @id", new { id = 1 });
        count++;
        if (count % 10_000 == 0)
            Console.WriteLine($"Task {taskId}: {count:N0} - {watch.Elapsed}");
    }
    watch.Stop();
    Console.WriteLine($"Task {taskId}: {count:N0} - {watch.Elapsed} - {count / watch.Elapsed.TotalSeconds:N1} op/sec - Fim");
    return count;
});

Task<int> StartWorkerTaskSync(int taskId) => Task.Run(() =>
{
    // Teste sem multiplos comandos por conexão
    using var conn = new SqlConnection($"Server=127.0.0.1;Database=teste;User Id=sa;Password=Pass123456;Application Name={appName};TrustServerCertificate=True;");
    conn.Open();
    var warmUpResult = conn.QueryFirst<Pessoa>("select * from pessoa where id = @id", new { id = 1 }); // Aquecer para libraries serem carregas para memória

    var watch = Stopwatch.StartNew();
    var count = 0;
    while (watch.Elapsed < totalTestTime)
    {
        var result = conn.QueryFirst<Pessoa>("select * from pessoa where id = @id", new { id = 1 });
        count++;
        if (count % 10_000 == 0)
            Console.WriteLine($"Task {taskId}: {count:N0} - {watch.Elapsed}");
    }
    watch.Stop();
    Console.WriteLine($"Task {taskId}: {count:N0} - {watch.Elapsed} - {count / watch.Elapsed.TotalSeconds:N1} op/sec - Fim");
    return count;
});

record Pessoa(int Id, string Nome);