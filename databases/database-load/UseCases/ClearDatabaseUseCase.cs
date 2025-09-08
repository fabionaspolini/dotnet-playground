using database_load_playground.Db;
using Spectre.Console;

namespace database_load_playground.UseCases;

public static class ClearDatabaseUseCase
{
    public static async Task ExecuteAsync()
    {
        AnsiConsole.Markup("[gray]Limpar banco de dados...[/]");
        await using var conn = await DbFactory.CreateConnectionAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "delete from transacao";
        await cmd.ExecuteNonQueryAsync();
        AnsiConsole.MarkupLine("[gray]OK[/]");
        
        AnsiConsole.Markup("[gray]Executar vacuum...[/]");
        cmd.CommandText = "vacuum";
        await cmd.ExecuteNonQueryAsync();
        AnsiConsole.MarkupLine("[gray]OK[/]");
    }
}