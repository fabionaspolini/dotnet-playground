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
            "1) Insert without transaction",
            "2) Insert with transaction",
            "3) Insert with multiples values",
            "4) Bulkt insert"));

AnsiConsole.MarkupLine($"Opção selecionada: [bold]{op}[/]");
AnsiConsole.WriteLine();

switch (op.SubstringBeforeFirstOccurrence(")"))
{
    case "1":
        await InsertUseCase.ExecuteAsync();
        break;
    case "2":
        await InsertWithTransactionUseCase.ExecuteAsync();
        break;
    // case "3":
    //     await UseCases.InsertWithMultiplesValues.Execute(conn);
    //     break;
    // case "4":
    //     await UseCases.BulkInsert.Execute(conn);
    //     break;
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