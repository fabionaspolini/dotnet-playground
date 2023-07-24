using Dapper;
using Npgsql;
using System.Diagnostics;

Console.WriteLine(".:: PostgreSQL Playground - Benchmark ::.");

Console.WriteLine("Iniciando tasks...");

//var tasks = new[] { StartWorkerTask(1) };
//var tasks = new[] { StartWorkerTask(1), StartWorkerTask(2) };

var tasks = new[]
{
    StartWorkerTask(1),
    StartWorkerTask(2),
    StartWorkerTask(3),
    StartWorkerTask(4),
    StartWorkerTask(5),
    StartWorkerTask(6),
};

Task.WaitAll(tasks);

var total = tasks.Sum(t => t.Result);

Console.WriteLine();
Console.WriteLine($"Total: {total:N0}");

Console.WriteLine();
Console.WriteLine("Fim");

Task<int> StartWorkerTask(int taskId) => Task.Run(async () =>
{
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