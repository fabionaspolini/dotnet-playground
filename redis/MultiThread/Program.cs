using StackExchange.Redis;
using System.Diagnostics;

Console.WriteLine(".:: Redis Playground - Multi Thread ::.");

Console.WriteLine("Conectando...");
var redis = ConnectionMultiplexer.Connect("localhost"); // Utilizar como singleton
redis.ErrorMessage += (object? sender, RedisErrorEventArgs e) =>
{
    Console.WriteLine("Redis error: " + e.Message);
};
redis.InternalError += (object? sender, InternalErrorEventArgs e) =>
{
    Console.WriteLine("Redis internal error: " + e.Exception?.ToString());
};
redis.ConnectionFailed += (object? sender, ConnectionFailedEventArgs e) =>
{
    // Se derrubar o Redis após o GetDatabase(), irá cair nesse controle de exceção
    Console.WriteLine($"Redis connection failed [{e} - {e.FailureType}]: {e.Exception}");
};
redis.ConnectionRestored += (object? sender, ConnectionFailedEventArgs e) =>
{
    Console.WriteLine($"Redis connection restored [{e}]");
};

var db = redis.GetDatabase(0);
Console.WriteLine("Conectado");
Console.WriteLine();

const string RedisKey = "multi-thread-sample";
await db.StringSetAsync(RedisKey, "Teste");

Console.WriteLine("Iniciando tasks...");
//var tasks = new[] { StartWorkerTask(1, db) };
//var tasks = new[] { StartWorkerTask(1, db), StartWorkerTask(2, db) };
//var tasks = new[] { StartWorkerTask(1, redis.GetDatabase(0)), StartWorkerTask(2, redis.GetDatabase(0)) };

var tasks = new[]
{
    StartWorkerTask(1, db),
    StartWorkerTask(2, db),
    StartWorkerTask(3, db),
    StartWorkerTask(4, db),
    StartWorkerTask(5, db),
    StartWorkerTask(6, db)
};
Task.WaitAll(tasks); // Multi thread seguro com a mesma conexão

var total = tasks.Sum(t => t.Result);

Console.WriteLine();
Console.WriteLine($"Total: {total:N0}");

Console.WriteLine();
Console.WriteLine("Fim");

Task<int> StartWorkerTask(int taskId, IDatabase db) => Task.Run(async () =>
{
    var watch = Stopwatch.StartNew();
    var time = TimeSpan.FromSeconds(10);
    var count = 0;
    while (watch.Elapsed < time)
    {
        //await db.StringSetAsync(RedisKey, "Teste");
        var result = db.StringGet(RedisKey);
        //var result = await db.StringGetAsync(RedisKey, flags: CommandFlags.FireAndForget);
        count++;
        if (count % 5_000 == 0)
            Console.WriteLine($"Task {taskId}: {count:N0} - {watch.Elapsed}");
    }
    watch.Stop();
    Console.WriteLine($"Task {taskId}: {count:N0} - {watch.Elapsed} - Fim");
    return count;
});
