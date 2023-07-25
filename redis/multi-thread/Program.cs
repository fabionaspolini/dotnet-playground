using StackExchange.Redis;
using System.Diagnostics;
using System.Threading.Tasks;

Console.WriteLine(".:: Redis Playground - Multi Thread ::.");

Console.WriteLine("Conectando...");

// Prevenção para ThreadPool não dar preferência para excecutar callback do usuário, penalizando consultas.
// Utilização de acordo com o SynchronizationContext da sua aplicação (Web, Desktop, Console, etc).
// https://stackexchange.github.io/StackExchange.Redis/ThreadTheft
// ConnectionMultiplexer.SetFeatureFlag("preventthreadtheft", true);

using var redis = ConnectionMultiplexer.Connect("localhost"); // Utilizar como singleton
redis.StormLogThreshold = 1_000_000;
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
const bool SetValueTest = true;
const bool FireAndForgetTest = true; // CommandFlags.FireAndForget / CommandFlags.None
TimeSpan TotalTestTime = TimeSpan.FromSeconds(10);
const int Threads = 10;

await db.KeyDeleteAsync(RedisKey);
if (!SetValueTest)
    await db.StringSetAsync(RedisKey, "Redis test");

Console.WriteLine("Iniciando tasks...");
var tasks = new List<Task<int>>();
foreach (var i in Enumerable.Range(1, Threads))
    tasks.Add(StartWorkerTask(i, db));

Task.WaitAll(tasks.ToArray());
var totalOperations = tasks.Sum(x => x.Result);

Console.WriteLine();
Console.WriteLine($"Total: {totalOperations:N0} - {totalOperations / TotalTestTime.TotalSeconds:N1} op/sec");

var redisCounters = redis.GetCounters();
if (FireAndForgetTest)
    Console.WriteLine($"Wait flushing memory queue ({redisCounters.Interactive.SentItemsAwaitingResponse:N0} itens)...");

db.HashIncrement(RedisKey, "total", tasks.Sum(x => x.Result), CommandFlags.FireAndForget);
SpinWait.SpinUntil(() => redisCounters.Interactive.SentItemsAwaitingResponse == 0, TimeSpan.FromSeconds(60)); // Nem sempre funciona, com muitas mensagens as vezes trava aqui

Console.WriteLine();
Console.WriteLine("Fim");
redis.Close();

Task<int> StartWorkerTask(int taskId, IDatabase db) => Task.Run(async () =>
{
    var watch = Stopwatch.StartNew();
    var count = 0;
    while (watch.Elapsed < TotalTestTime)
    {
        // db = redis.GetDatabase(0); // Thread safe obter para obter a conexão dentro do scope de execução;
        if (SetValueTest)
        {
            if (FireAndForgetTest)
                db.HashIncrement(RedisKey, taskId, 1, CommandFlags.FireAndForget);
            else
                await db.HashIncrementAsync(RedisKey, taskId, 1, CommandFlags.None);
        }
        else
        {
            if (FireAndForgetTest)
                db.StringGet(RedisKey, CommandFlags.FireAndForget);
            else
                await db.StringGetAsync(RedisKey);
        }
        count++;
        if (count % 5_000 == 0)
        {
            Console.WriteLine($"Task {taskId}: {count:N0} - {watch.Elapsed}");

            // No teste de carga a fila do "Fire and Forget" estoura sem ter um espaço para descarregamento.
            // Com 10 threads e 50ms de delay a cada 5k registros, foi possível atingir a taxa de 749.973,3 op/sec (74.593,4 op/sec/thread)
            if (FireAndForgetTest)
                await Task.Delay(50);
        }
    }
    watch.Stop();
    Console.WriteLine($"Task {taskId}: {count:N0} - {watch.Elapsed} - {count / watch.Elapsed.TotalSeconds:N1} op/sec - Fim");
    return count;
});
