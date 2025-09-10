using database_load_playground.Db;
using Spectre.Console;

namespace database_load_playground.UseCases;

public static class ClearDatabaseUseCase
{
    public static async Task ExecuteAsync(bool clearTransacao, bool clearTransacaoPart)
    {
        await using var conn = await DbFactory.CreateConnectionAsync();
        await using var cmd = conn.CreateCommand();

        if (clearTransacao)
        {
            AnsiConsole.Markup("[gray]Limpar banco de dados - transacao...[/]");
            cmd.CommandText = "truncate transacao";
            await cmd.ExecuteNonQueryAsync();
        }

        if (clearTransacaoPart)
        {
            AnsiConsole.Markup("[gray]Limpar banco de dados - transacao_part...[/]");
            cmd.CommandText = "truncate transacao_part";
            await cmd.ExecuteNonQueryAsync();
        }

        if (clearTransacao || clearTransacaoPart)
            AnsiConsole.MarkupLine("[gray]OK[/]");
        
        AnsiConsole.Markup("[gray]Executar vacuum...[/]");
        cmd.CommandText = "vacuum";
        await cmd.ExecuteNonQueryAsync();
        AnsiConsole.MarkupLine("[gray]OK[/]");
    }
}