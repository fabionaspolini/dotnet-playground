using System;
using System.Threading.Tasks;
using CacheManager.Core;
using Microsoft.Extensions.Logging;

var loggerFactory = LoggerFactory.Create(builder => builder
    .AddFilter("CacheManager", LogLevel.Trace)
    .AddFilter("CacheReflectionHelper", LogLevel.Trace)
    .AddSimpleConsole(opts =>
    {
        opts.SingleLine = true;
        opts.IncludeScopes = true;
        opts.TimestampFormat = "dd/MM/yyyy HH:mm:ss.fff ";
    }));
var logger = loggerFactory.CreateLogger("CachaSample");
logger.LogInformation(".:: Cache Manager Samples ::.");

// Criando configuração de log reaproveitável para N objetos de cache
var config = new ConfigurationBuilder()
    .WithUpdateMode(CacheUpdateMode.Up)
    .WithJsonSerializer()
    .WithRedisConfiguration("redis", "localhost:6379,ssl=false")
    .WithMicrosoftLogging(loggerFactory)
    .WithSystemRuntimeCacheHandle("memory")
        .WithExpiration(ExpirationMode.Sliding, TimeSpan.FromSeconds(5)) // Sliding => Expirar cache após 5 segundos sem acesso
    .And.WithRedisCacheHandle("redis")
        .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromSeconds(120)) // Absolute => Expirar cache 120 segundos após criação
    .Build();

// Objeto gestor de cache para classe "Pessoa"
var pessoaCache = new BaseCacheManager<Pessoa>(config);

logger.LogInformation("Get 1" + new string('-', 100));
var pessoaGet1 = pessoaCache.GetOrAdd("pessoa", key => PessoaFactory.Create(logger)); // Irá criar a Pessoa. Ao executar novamente o demo antes de 2 minutos, obterá do redis.
logger.LogInformation("Get 2" + new string('-', 100));
var pessoaGet2 = pessoaCache.GetOrAdd("pessoa", key => PessoaFactory.Create(logger)); // Caso estiver executando pela primeira vez o demo, neste momento que o cache do redis será sincronizado para a memória da RAM. Caso já existi-se o cache na execuçaõ do Get 1, na linha acima estaria armazenando também na memória
logger.LogInformation("Get 3" + new string('-', 100));
var pessoaGet3 = pessoaCache.GetOrAdd("pessoa", key => PessoaFactory.Create(logger));

logger.LogInformation("Get 4" + new string('-', 100));
logger.LogInformation("Delay 3 segundos...");
await Task.Delay(3000);
var pessoaGet4 = pessoaCache.GetOrAdd("pessoa", key => PessoaFactory.Create(logger)); // Aqui o cache continuar na memória

logger.LogInformation("Get 5" + new string('-', 100));
logger.LogInformation("Delay 3 segundos...");
await Task.Delay(3000);
var pessoaGet5 = pessoaCache.GetOrAdd("pessoa", key => PessoaFactory.Create(logger)); // Aqui o cache continuar na memória

logger.LogInformation("Get 6" + new string('-', 100));
logger.LogInformation("Delay 6 segundos...");
await Task.Delay(6000);
var pessoaGet6 = pessoaCache.GetOrAdd("pessoa", key => PessoaFactory.Create(logger)); // Aqui o cache já foi removido da memória porque está com a configuração "Slidind" em 5 segundos, mas será obtido do Redis e resincronizado para memória

logger.LogInformation(new string('=', 100));
logger.LogInformation($"Pessoa: {pessoaGet1.Nome} | {pessoaGet1.DataCadastro}");
await Task.Delay(1000); // Para dar tempo do ILogger descarregar tudo no console

// Outros exemplos
logger.LogInformation(new string('=', 100));
logger.LogInformation("Testes para Add, Put, Update e Remove");
var cache = new BaseCacheManager<object>(config);
cache.Add("keyA", "valueA");
cache.Put("keyB", 41);
cache.Update("keyB", v => (int)v + 1);

logger.LogInformation("KeyA is " + cache.Get("keyA")); // Deve ser valueA
logger.LogInformation("KeyB is " + cache.Get("keyB")); // Deve ser 42
cache.Remove("keyA");

logger.LogInformation("KeyA removed? " + (cache.Get("keyA") == null));

logger.LogInformation("KeyB is expired? " + (cache.Get("keyB") == null));
logger.LogInformation("KeyB is " + cache.Get("keyB")); // Deve ser 42

await Task.Delay(1000); // Para dar tempo do ILogger descarregar tudo no console

public class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public DateTime DataCadastro { get; set; }
    public override string ToString() => $"{Id}|{Nome}|{DataCadastro:dd/MM/yyy HH:mm:ss}";
}

public static class PessoaFactory
{
    public static Pessoa Create(ILogger logger)
    {
        logger.LogInformation(">>> Gerando nova pessoa");
        return new Pessoa
        {
            Id = 1,
            Nome = "Fulano",
            DataCadastro = DateTime.Now
        };
    }
}

// Alternativa para configurar e instânciar cache inline
/*var pessoaCache = CacheFactory.Build<Pessoa>(settings => settings
    .WithUpdateMode(CacheUpdateMode.Up)
    .WithJsonSerializer()
    .WithRedisConfiguration("redis", "localhost:6379,ssl=false")
    .WithMicrosoftLogging(loggerFactory)
    .WithSystemRuntimeCacheHandle("memory")
        .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromSeconds(60))
    .And.WithRedisCacheHandle("redis")
        .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromSeconds(120)));*/