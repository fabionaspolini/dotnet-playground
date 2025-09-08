using System.Diagnostics;
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

await ClearDatabaseUseCase.ExecuteAsync();
AnsiConsole.WriteLine();

// Menu
var op = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Qual caso de uso executar?")
        .AddChoices(
            "1) Insert without transaction -> 10 mil",
            "2) Insert with transaction -> 10 mil",
            "3.1) Insert with multiples values -> 10 mil",
            "3.2) Insert with multiples values -> 1 milhão",
            "3.3) Insert with multiples values -> 1 milhão em pacotes de 5 mil",
            "4) Parallel insert (WTF!)",
            "5.1) Insert with copy -> 1 milhão",
            "5.2) Insert with copy -> 1 milhão em pacotes de 5 mil",
            "5.3) Insert with copy -> 10 milhões",
            "5.4) Insert with copy -> 10 milhões em pacotes de 5 mil",
            "5.5) Insert with copy -> 10 milhões em pacotes de 50 mil",
            "10) Insert or update -> 10 mil"
        ));

AnsiConsole.MarkupLine($"Opção selecionada: [bold]{op}[/]");

switch (op.SubstringBeforeFirstOccurrence(")"))
{
    case "1":
        await InsertUseCase.ExecuteAsync(10_000);
        break;
    case "2":
        await InsertWithTransactionUseCase.ExecuteAsync(10_000);
        break;
    case "3.1":
        await InsertWithMultiplesValuesUseCase.ExecuteAsync(10_000);
        break;
    case "3.2":
        await InsertWithMultiplesValuesUseCase.ExecuteAsync(1_000_000);
        break;
    case "3.3":
        await InsertWithMultiplesValuesUseCase.ExecuteAsync(1_000_000, 5_000);
        break;
    case "5.1":
        await InsertWithCopyUseCase.ExecuteAsync(1_000_000);
        break;
    case "5.2":
        await InsertWithCopyUseCase.ExecuteAsync(1_000_000, 5_000);
        break;
    case "5.3":
        await InsertWithCopyUseCase.ExecuteAsync(10_000_000);
        break;
    case "5.4":
        await InsertWithCopyUseCase.ExecuteAsync(10_000_000, 5_000);
        break;
    case "5.5":
        await InsertWithCopyUseCase.ExecuteAsync(10_000_000, 50_000);
        break;
    case "10":
        await InsertOrUpdateUseCase.ExecuteAsync(10_000);
        break;
    default:
        AnsiConsole.MarkupLine("[red]Opção inválida![/]");
        break;
}

namespace database_load_playground
{
    public static class Consts
    {
        public const string ConnectionString = "Server=localhost;Port=5432;Database=load-playground;User Id=postgres;Password=123456;";
    }
}