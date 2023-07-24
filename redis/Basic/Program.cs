using StackExchange.Redis;

Console.WriteLine(".:: Redis Playground - Basic ::.");

// https://stackexchange.github.io/StackExchange.Redis/Basics

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
Console.WriteLine("OK");

Console.WriteLine("Gravando...");
await db.StringSetAsync($"Pessoa:1", "Fulano", TimeSpan.FromSeconds(15), flags: CommandFlags.FireAndForget); // Com FireForget, não ocorrerá erro mesmo que não consiga gravar

// Thread.Sleep(15000); // Para simular eventos de queda do servidor após conexão

await db.StringSetAsync($"Pessoa:2", "Ciclano", TimeSpan.FromSeconds(15));
await db.StringSetAsync($"Pessoa:3", "Beltrano", TimeSpan.FromSeconds(15));
await db.StringSetAsync($"Pessoa:1", "Fulano de tal", TimeSpan.FromSeconds(15), when: When.NotExists); // Atribuir valor somente se não existir a chave
Console.WriteLine("OK");
Console.WriteLine();

Console.WriteLine("Consultando...");
var pessoa = await db.StringGetAsync("Pessoa:1", CommandFlags.PreferReplica); // Preferência por réplica, mas não da erro se não existir réplica. Se utilizar "DemandReplica" é obirgatória a presença de instâncias de leitura
Console.WriteLine($"Pessoa 1: {pessoa}");
var pessoa99999 = await db.StringGetAsync("Pessoa:99999");
Console.WriteLine(pessoa99999.HasValue ? pessoa99999 : "Pessoa 99999 null");
Console.WriteLine("OK");
Console.WriteLine();

// ##### Contadores início #####

Console.WriteLine("Contadores...");
var meuContatdorValue = await db.StringIncrementAsync("MeuContador", 1); // TTL infinito
//var meuContatdorValue = await db.StringGetAsync("MeuContador"); // Ler se incrementar
Console.WriteLine($"MeuContador: {meuContatdorValue}");

var hashCount1 = await db.HashIncrementAsync("MeuContadorHash", "Info1", 1); // Incrementar e obter valor
var hashCount2 = await db.HashIncrementAsync("MeuContadorHash", "Info2", 2);
var hashCount3 = await db.HashIncrementAsync("MeuContadorHash", "Info3", 3);
//var hashCount1 = await db.HashGetAsync("MeuContadorHash", "Info1"); // Ler se incrementar

var hashsCountAll = await db.HashGetAllAsync("MeuContadorHash");
foreach (var item in hashsCountAll)
    Console.WriteLine($"MeuContadorHash[{item.Name}]: {item.Value}");
Console.WriteLine();

// Contadore ordenado descrescente pelo redis
var hashSorted1 = await db.SortedSetIncrementAsync("MeuContadorSorted", "Info1", 1); // Incrementar e obter valor
var hashSorted2 = await db.SortedSetIncrementAsync("MeuContadorSorted", "Info2", 2);
var hashSorted3 = await db.SortedSetIncrementAsync("MeuContadorSorted", "Info3", 3);

var hashsSorted = await db.SortedSetRangeByRankWithScoresAsync("MeuContadorSorted", 0, 1, Order.Descending); // Obter dois maiores scores
foreach (var item in hashsSorted)
    Console.WriteLine($"Range by rank 0..1 => {item.Element}: {item.Score}");
Console.WriteLine();

var rank = await db.SortedSetRankAsync("MeuContadorSorted", "Info3", Order.Descending);
Console.WriteLine($"Rank of MeuContadorSorted.Info3: {rank}");

Console.WriteLine("OK");
Console.WriteLine();

// ##### Contadores fim #####

// Alterar TTL da chave. Utilizado para Sliding expiration.
await db.KeyExpireAsync("Pessoa:2", TimeSpan.FromSeconds(60));

// Excluir chave
await db.KeyDeleteAsync("Pessoa:3");

Console.WriteLine("Fim");


// Exception quando o servidor está inacessível
//StackExchange.Redis.RedisConnectionException: 'The message timed out in the backlog attempting to send because no
//connection became available (5000ms) - Last Connection Exception: It was not possible to connect to the redis server(s).
//Error connecting right now. To allow this multiplexer to continue retrying until it's able to connect,
//use abortConnect=false in your connection string or AbortOnConnectFail=false; in your code.
//ConnectTimeout, command = SETEX, timeout: 5000, inst: 0, qu: 0, qs: 0, aw: False, bw: CheckingForTimeout,
//rs: NotStarted, ws: Idle, in: 0, last -in: 0, cur -in: 0, sync - ops: 0, async - ops: 1,
//serverEndpoint: localhost: 6379, conn - sec: n / a, aoc: 1, mc: 1 / 1 / 0, mgr: 10 of 10 available,
//clientName: FABIO - PC(SE.Redis - v2.6.122.38350), IOCP: (Busy = 0, Free = 1000, Min = 1, Max = 1000),
//WORKER: (Busy = 0, Free = 32767, Min = 12, Max = 32767),
//POOL: (Threads = 6, QueuedItems = 0, CompletedItems = 88, Timers = 3),
//v: 2.6.122.38350(Please take a look at this article for some common client -
//side issues that can cause timeouts: https://stackexchange.github.io/StackExchange.Redis/Timeouts)'
