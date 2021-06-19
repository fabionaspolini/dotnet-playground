using System;
using CacheManager.Core;
using static System.Console;

WriteLine(".:: Cache Manager Samples ::.");

var cache = CacheFactory.Build<Pessoa>("getStartedCache",
    settings => settings
        .WithUpdateMode(CacheUpdateMode.Up)
            .WithSystemRuntimeCacheHandle("handleName")
            .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromSeconds(60))
        .And.WithRedisConfiguration("redisLocal", "localhost:6379,ssl=false")
            .WithRedisCacheHandle("redisLocal")
            .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromSeconds(120))
        .And.WithJsonSerializer());

var pessoa = cache.GetOrAdd("pessoa", key => PessoaFactory.Create());
WriteLine($"Pessoa: {pessoa.Nome} | {pessoa.DataCadastro}");
WriteLine();

/*cache.Add("keyA", "valueA");
cache.Put("keyB", 23);
cache.Update("keyB", v => Teste.GetValue());

WriteLine("KeyA is " + cache.Get("keyA"));      // should be valueA
WriteLine("KeyB is " + cache.Get("keyB"));      // should be 42
cache.Remove("keyA");

WriteLine("KeyA removed? " + (cache.Get("keyA") == null).ToString());

Thread.Sleep(TimeSpan.FromSeconds(2));
WriteLine("KeyB is expired? " + (cache.Get("keyB") == null).ToString());
WriteLine("KeyB is " + cache.Get("keyB"));      // should be 42

var testeC = cache.Get("keyC");
testeC = cache.Get("keyC");

WriteLine("KeyC: " + cache.GetOrAdd("keyC", (key) => Teste.GetValue(key)));
WriteLine("KeyC: " + cache.GetOrAdd("keyC", (key) => Teste.GetValue(key)));
WriteLine("KeyC: " + cache.GetOrAdd("keyC", (key) => Teste.GetValue(key)));

WriteLine("Pessoa: " + cache.GetOrAdd("pessoa", key => PessoaFactory.Create()));

WriteLine("We are done...");*/

public static class Teste
{
    public static int GetValue() => 50;
    public static int GetValue(string key) => 51;
}

public class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public DateTime DataCadastro { get; set; }
    public override string ToString() => $"{Id}|{Nome}|{DataCadastro:dd/MM/yyy HH:mm:ss}";
}

public static class PessoaFactory
{
    public static Pessoa Create()
    {
        WriteLine(">>> Gerando nova pessoa");
        return new Pessoa
        {
            Id = 1,
            Nome = "Fulano",
            DataCadastro = DateTime.Now
        };
    }
}