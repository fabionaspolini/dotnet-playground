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
}

public static class TransacaoFactory
{
    public static Faker<Transacao> Faker { get; } = new Faker<Transacao>()
        .StrictMode(true)
        .RuleFor(x => x.Id, f => Guid.CreateVersion7())
        .RuleFor(x => x.Data, f => f.Date.Past(yearsToGoBack: 1))
        .RuleFor(x => x.ClienteId, f => Guid.CreateVersion7())
        .RuleFor(x => x.Valor, f => f.Finance.Amount(0.01m, 10_000m))
        .RuleFor(x=> x.Descricao, f => f.Lorem.Text().Truncate(40));
    
    public static List<Transacao> Generate(int count)
    {
        const int chunkSize = 10_000;
        var result = new List<Transacao>(count);
        
        AnsiConsole.Progress()
            .Start(ctx =>
            {
                var task = ctx.AddTask("[gray]Gerando dados fake[/]", maxValue: count);
                while (!ctx.IsFinished)
                {
                    while (result.Count + chunkSize < count)
                    {
                        result.AddRange(Faker.Generate(chunkSize));
                        task.Increment(chunkSize); 
                    }
                    if (result.Count < count)
                    {
                        var lastChunk = count - result.Count;
                        result.AddRange(Faker.Generate(lastChunk));
                        task.Increment(lastChunk); 
                    }
                }
            });
        return result;

        /*AnsiConsole.Markup("[gray]Gerar dados fake...[/]");
        try
        {
            return Faker.Generate(count);
        }
        finally
        {
            AnsiConsole.MarkupLine("[gray]OK[/]");
        }*/
    }
}