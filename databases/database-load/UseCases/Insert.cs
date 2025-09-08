using System.Diagnostics;
using Bogus;
using Dapper.Contrib.Extensions;
using database_load_playground.Db;
using database_load_playground.Entities;
using Maestria.Extensions;
using Spectre.Console;

namespace database_load_playground.UseCases;

public class InsertUseCase
{
    private const string Sql =
        """
        insert into transacao (id, cliente_id, valor, descricao)
        values (@id, @cliente_id, @valor, @descricao)
        """;
    public async Task ExecuteAsync()
    {
        await using var conn = await DbFactory.CreateConnectionAsync();
        var itens = TransacaoFactory.Generate();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = Sql;

        var watch = Stopwatch.StartNew();
        foreach (var item in itens)
        {
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("id", item.Id);
            cmd.Parameters.AddWithValue("cliente_id", item.ClienteId);
            cmd.Parameters.AddWithValue("valor", item.Valor);
            cmd.Parameters.AddWithValue("descricao", item.Descricao);
            await cmd.ExecuteNonQueryAsync();
        }
        watch.Stop();
        AnsiConsole.MarkupLine($"[green]{itens.Count:N0}[/] registros inseridos em [green]{watch.Elapsed}[/]");
    }
}