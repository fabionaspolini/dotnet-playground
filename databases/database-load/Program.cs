using System.Diagnostics;
using System.Globalization;
using database_load_playground;
using database_load_playground.Db;
using database_load_playground.UseCases;
using Maestria.Extensions;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

if (Debugger.IsAttached)
{
    var ansiConsoleSettings = new AnsiConsoleSettings { Ansi = AnsiSupport.Yes }; // Rider debug console fix
    AnsiConsole.Console = AnsiConsole.Create(ansiConsoleSettings);
}

AnsiConsole.MarkupLine("[bold yellow].:: Database Load Playground ::.[/]");

var optionsBuilder = new DbContextOptionsBuilder<LoadContext>();
optionsBuilder.UseNpgsql(Consts.ConnectionString).UseSnakeCaseNamingConvention();
// optionsBuilder.LogTo(Console.WriteLine);

// Usar EF Migrations para criar banco de dados e tabelas
AnsiConsole.Markup("[gray]Aplicando Migrations...[/]");
await using var conn = new LoadContext(optionsBuilder.Options);
await conn.Database.MigrateAsync();
AnsiConsole.MarkupLine("[gray]OK[/]");

// Warm up test
AnsiConsole.Markup("[gray]Warm up frameworks...[/]");
await conn.Transacoes.FirstOrDefaultAsync();
await using (var conn2 = await DbFactory.CreateConnectionAsync())
{
    await using var cmd = conn2.CreateCommand();
    cmd.CommandText = "select * from transacao limit 10";
    await using var reader = await cmd.ExecuteReaderAsync();
    while (await reader.ReadAsync())
    {
    }
}
AnsiConsole.MarkupLine("[gray]OK[/]");
AnsiConsole.WriteLine();

const string menuSair = "Sair";
string op;
do
{
    PrintLine("Selecione o caso de teste");
    AnsiConsole.WriteLine();
    
    // Menu
    op = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            // .Title("Selecione o caso de teste")
            .AddChoices(
                "1) Insert without transaction -> 10 mil",
                "2) Insert with transaction -> 10 mil",
                "3.1) Bulk insert with multiples values -> 10 mil",
                "3.2) Bulk insert with multiples values -> 1 milhão",
                "3.3) Bulk insert with multiples values -> 1 milhão em pacotes de 5 mil",
                "4) Parallel insert (WTF!)",
                "5.1) Bulk insert with copy -> 1 milhão",
                "5.2) Bulk insert with copy -> 1 milhão em pacotes de 5 mil",
                "5.3) Bulk insert with copy -> 10 milhões",
                "5.4) Bulk insert with copy -> 10 milhões em pacotes de 5 mil",
                "5.5) Bulk insert with copy -> 10 milhões em pacotes de 50 mil",
                "5.6) Bulk insert with copy -> + 5 milhões em pacotes de 50 mil (sem limpar dados)",
                "10.1) Insert or update -> 10 mil",
                "10.2) Insert or update -> 50 mil",
                "10.3) Insert or update -> 200 mil",
                "11.1) Upsert -> 10 mil",
                "11.2) Upsert -> 50 mil",
                "11.3) Upsert -> 200 mil",
                "12.1) Bulk upsert -> 10 mil",
                "12.2) Bulk upsert -> 200 mil",
                "12.3) Bulk upsert -> 1 milhão",
                "20.1) Bulk upsert partitioned table -> 200 mil",
                "20.2) Bulk upsert partitioned table -> 1 milhão",
                menuSair
            ));
    // Pendentes
    // copy para extrair dados
    
    AnsiConsole.MarkupLine($"Opção selecionada: [bold]{op}[/]");
    if (op != menuSair)
    {
        var opNumberText = op.SubstringBeforeFirstOccurrence(")");
        var opNumberValue = Convert.ToDecimal(opNumberText, CultureInfo.InvariantCulture);
        await ClearDatabaseUseCase.ExecuteAsync(
            clearTransacao: opNumberValue < 20 && opNumberText != "5.6",
            clearTransacaoPart: opNumberValue >= 20);
        
        switch (opNumberText)
        {
            case "1": await InsertUseCase.ExecuteAsync(10_000); break;
            case "2": await InsertWithTransactionUseCase.ExecuteAsync(10_000); break;
            case "3.1": await BulkInsertWithMultiplesValuesUseCase.ExecuteAsync(10_000); break;
            case "3.2": await BulkInsertWithMultiplesValuesUseCase.ExecuteAsync(1_000_000); break;
            case "3.3": await BulkInsertWithMultiplesValuesUseCase.ExecuteAsync(1_000_000, 5_000); break;
            case "5.1": await BulkInsertWithCopyUseCase.ExecuteAsync(1_000_000); break;
            case "5.2": await BulkInsertWithCopyUseCase.ExecuteAsync(1_000_000, 5_000); break;
            case "5.3": await BulkInsertWithCopyUseCase.ExecuteAsync(10_000_000); break;
            case "5.4": await BulkInsertWithCopyUseCase.ExecuteAsync(10_000_000, 5_000); break;
            case "5.5": await BulkInsertWithCopyUseCase.ExecuteAsync(10_000_000, 50_000); break;
            case "5.6": await BulkInsertWithCopyUseCase.ExecuteAsync(5_000_000, 50_000); break;
            case "10.1": await InsertOrUpdateUseCase.ExecuteAsync(10_000); break;
            case "10.2": await InsertOrUpdateUseCase.ExecuteAsync(50_000); break;
            case "10.3": await InsertOrUpdateUseCase.ExecuteAsync(200_000); break;
            case "11.1": await UpsertUseCase.ExecuteAsync(10_000); break;
            case "11.2": await UpsertUseCase.ExecuteAsync(50_000); break;
            case "11.3": await UpsertUseCase.ExecuteAsync(200_000); break;
            case "12.1": await BulkUpsertUseCase.ExecuteAsync(10_000); break;
            case "12.2": await BulkUpsertUseCase.ExecuteAsync(200_000); break;
            case "12.3": await BulkUpsertUseCase.ExecuteAsync(1_000_000); break;
            case "20.1": await BulkUpsertPartitionedTableUseCase.ExecuteAsync(200_000); break;
            case "20.2": await BulkUpsertPartitionedTableUseCase.ExecuteAsync(1_000_000); break;
            default: AnsiConsole.MarkupLine("[red]Opção inválida![/]"); break;
        }

        AnsiConsole.WriteLine();
    }
} while(op != menuSair);

void PrintLine(string text)
{
    var rule = new Rule($"[blue][b]{text}[/][/]")
    {
        Justification = Justify.Left,
        Border = BoxBorder.Heavy,
    };
    AnsiConsole.Write(rule);
}