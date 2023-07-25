using System.Diagnostics;
using EFCore_ZExtensionsPlayground.Entities;
using EFCore_ZExtensionsPlayground.Infra;
using Microsoft.EntityFrameworkCore;

namespace EFCore_ZExtensionsPlayground;

class Program
{
    public const string ConnectioString = "Server=localhost;Port=5432;Database=ef-core-z-extensions-sample;User Id=postgres;Password=123456;Application Name=EF Core Z Extensions Sample";
    static void Main(string[] args)
    {
        Console.WriteLine(".:: EF Core - Z Extensions Sample ::.");

        var optionsBuilder = new DbContextOptionsBuilder<SampleContext>();
        optionsBuilder.UseNpgsql(ConnectioString);
        // optionsBuilder.LogTo(Console.WriteLine);

        // Para usar a opção IncludeGraph = true
        // Z.EntityFramework.Extensions.EntityFrameworkManager.ContextFactory = context =>
        //    new SampleContext(new DbContextOptionsBuilder<SampleContext>().UseNpgsql(ConnectioString).Options);

        using var conn = new SampleContext(optionsBuilder.Options);
        conn.Database.Migrate();

        // Apagar registros com base na query
        conn.Database.ExecuteSqlRaw("alter table cliente set (autovacuum_enabled = false)");
        conn.Database.ExecuteSqlRaw("delete from cliente");
        conn.Database.ExecuteSqlRaw("vacuum full");

        // Inserção em massa
        conn.Pessoas.BulkInsert(new[]
        {
            new Pessoa(1, "João", "000", "São Paulo"),
            new Pessoa(2, "Paulo", "000", "São Paulo"),
            new Pessoa(3, "Pedro", "000", "São Paulo"),
        });

        // Atualização em massa - Registro 4 é ignorado porque não existe
        conn.Pessoas.BulkUpdate(new[]
        {
            new Pessoa(1, "João", "111", "São Paulo"),
            new Pessoa(2, "Paulo", "222", "São Paulo"),
            new Pessoa(4, "Marcos", "444", "São Paulo"),
        });

        // Insert or update
        conn.Pessoas.BulkMerge(new[]
        {
            new Pessoa(1, "João", "111", "Florianópolis"),
            new Pessoa(2, "Paulo", "222", "Florianópolis"),
            new Pessoa(3, "Pedro", "333", "Florianópolis"),
            new Pessoa(4, "Pedro", "444", "Florianópolis"),
        });

        // Quando há muitos registros, é utilizado a sintax: COPY "cliente" ("Id","Cidade","Documento","Nome") FROM STDIN BINARY
        conn.BulkInsert(GetPessoas(10, 20, "Teste", "Teste"));
        conn.BulkUpdate(GetPessoas(10, 20, "Nome", "Cidade"));
        conn.BulkMerge(GetPessoas(10, 1_000_000, "Nome upsert", "Cidade upsert"));

        // Performance
        var registrosInsert = GetPessoas(2_000_001, 3_000_000, "Perf insert", "Perf insert").ToArray();
        var registrosMerge = GetPessoas(3_000_001, 4_000_000, "Perf merge", "Perf merge").ToArray();
        var registrosMergeInsertAndUpdate = GetPessoas(3_500_001, 4_500_000, "Perf merge", "insert").ToArray();

        var watch = Stopwatch.StartNew();

        Console.WriteLine($"BulkInsert...");
        watch.Restart();
        conn.BulkInsert(registrosInsert);
        Console.WriteLine($"BulkInsert: {watch.Elapsed}"); // 1 milhão de registros inseridos em 00:00:04.1588032

        Console.WriteLine($"BulkMerge...");
        watch.Restart();
        conn.BulkMerge(registrosMerge);
        Console.WriteLine($"BulkMerge: {watch.Elapsed}"); // 1 milhão de inserts via merge em 00:00:14.2118215

        Console.WriteLine($"BulkMerge (insert and update)...");
        watch.Restart();
        conn.BulkMerge(registrosMergeInsertAndUpdate);
        Console.WriteLine($"BulkMerge (insert and update): {watch.Elapsed}"); // 500 mil inseridos + 500 mil atualizados via merge em 00:00:09.7409390

        watch.Stop();
        Console.WriteLine("Fim");
    }

    static IEnumerable<Pessoa> GetPessoas(int startId, int endId, string nome, string cidade)
    {
        for (var i = startId; i <= endId; i++)
            yield return new Pessoa(i, $"{nome} {i}", i.ToString(), $"{cidade} {i}");
    }
}
