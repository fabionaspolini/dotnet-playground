using Dapper;
using Npgsql;
using System.Diagnostics;

Console.WriteLine(".:: PostgreSQL Playground - Benchmark ::.");

TimeSpan TotalTestTime = TimeSpan.FromSeconds(10);

Console.WriteLine("Iniciando tasks...");
var tasks = new List<Task<int>>();
foreach (var i in Enumerable.Range(1, 10))
    tasks.Add(StartWorkerTask(i));

Task.WaitAll(tasks.ToArray());
var totalOperations = tasks.Sum(t => t.Result);

Console.WriteLine();
Console.WriteLine($"Total: {totalOperations:N0} - {totalOperations / TotalTestTime.TotalSeconds:N1} op/sec");

Console.WriteLine();
Console.WriteLine("Fim");

Task<int> StartWorkerTask(int taskId) => Task.Run(async () =>
{
    // PostgreSQL não suporta multiplos comandos por conexão, sendo necessário abrir uma connection em cada thread.
    var conn = new NpgsqlConnection("Server=127.0.0.1;Port=5432;Database=teste;User Id=postgres;Password=123456;");
    await conn.OpenAsync();

    var watch = Stopwatch.StartNew();
    var time = TimeSpan.FromSeconds(10);
    var count = 0;
    while (watch.Elapsed < time)
    {
        var result = await conn.QueryFirstAsync<Pessoa>("select * from pessoa where id = :id", new { id = 1 });
        count++;
        if (count % 1_000 == 0)
            Console.WriteLine($"Task {taskId}: {count:N0} - {watch.Elapsed}");
    }
    watch.Stop();
    Console.WriteLine($"Task {taskId}: {count:N0} - {watch.Elapsed} - Fim");
    return count;
});

record Pessoa(int Id, string Nome);