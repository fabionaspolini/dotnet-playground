using System.Diagnostics;
using Bogus;
using Maestria.Extensions;
using Spectre.Console;

namespace database_load_playground.Entities;

public class Transacao
{
    public Guid Id { get; set; }
    public DateTime Data { get; set; }
    public Guid ClienteId { get; set; }
    public decimal Valor { get; set; }
    public string Descricao { get; set; } = null!;
    public DateTime? DataAtualizacao { get; set; }
}

public static class TransacaoFactory
{
    public static Faker<Transacao> Faker { get; } = new Faker<Transacao>("pt_BR")
        .StrictMode(true)
        .RuleFor(x => x.Id, f => Guid.CreateVersion7())
        .RuleFor(x => x.Data, f => f.Date.Past(yearsToGoBack: 1))
        .RuleFor(x => x.ClienteId, f => Guid.CreateVersion7())
        .RuleFor(x => x.Valor, f => f.Finance.Amount(0.01m, 10_000m))
        .RuleFor(x => x.Descricao, f => f.Lorem.Text().Truncate(40))
        .Ignore(x => x.DataAtualizacao);

    public static List<Transacao> Generate(int count)
    {
        const int chunkSize = 10_000;
        
        var result = new List<Transacao>();

        var watch = Stopwatch.StartNew();
        AnsiConsole.Progress()
            .AutoClear(true)
            .Start(ctx =>
            {
                var consoleProgressTask = ctx.AddTask("[gray]Gerando dados fake[/]", maxValue: count);
                
                while (!ctx.IsFinished)
                {
                    while (result.Count + chunkSize < count)
                    {
                        result.AddRange(Faker.Generate(chunkSize));
                        consoleProgressTask.Increment(chunkSize);
                    }

                    if (result.Count < count)
                    {
                        var lastChunk = count - result.Count;
                        result.AddRange(Faker.Generate(lastChunk));
                        consoleProgressTask.Increment(lastChunk);
                    }
                }
            });
        watch.Stop();
        AnsiConsole.MarkupLine($"[gray]{count:N0} dados fake gerados em {watch.Elapsed}[/]");
        return result.ToList();
    }
    
    /*public static List<Transacao> Generate(int count)
    {
        const int chunkSize = 10_000;
        var jobs = count / chunkSize;
        var extraItems = count % chunkSize;
        
        var result = new ConcurrentBag<Transacao>();

        var watch = Stopwatch.StartNew();
        AnsiConsole.Progress()
            .AutoClear(true)
            .Start(ctx =>
            {
                var consoleProgressTask = ctx.AddTask("[gray]Gerando dados fake[/]", maxValue: count);

                var semaphore = new SemaphoreSlim(25);
                var tasks = new List<Task>();

                for (var i = 0; i < jobs; i++)
                    tasks.Add(InternalGenerate(chunkSize, result, semaphore, consoleProgressTask));
                if (extraItems > 0)
                    tasks.Add(InternalGenerate(extraItems, result, semaphore, consoleProgressTask));

                Task.WaitAll(tasks);
            });
        watch.Stop();
        AnsiConsole.MarkupLine($"[gray]{count:N0} dados fake gerados em {watch.Elapsed} por {jobs} tasks[/]");
        return result.ToList();
    }

    private static Task InternalGenerate(int count, ConcurrentBag<Transacao> result, SemaphoreSlim semaphore, ProgressTask consoleProgressTask)
    {
        semaphore.Wait();
        return Task.Factory.StartNew(() =>
        {
            try
            {
                var items = Faker.Generate(count);
                items.ForEach(result.Add);
                consoleProgressTask.Increment(items.Count);
            }
            finally
            {
                semaphore.Release();
            }
        });
    }*/
}