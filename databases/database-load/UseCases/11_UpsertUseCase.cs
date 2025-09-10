using System.Diagnostics;
using database_load_playground.Db;
using database_load_playground.Entities;
using Maestria.Extensions;
using Spectre.Console;

namespace database_load_playground.UseCases;

public static class UpsertUseCase
{
    private const string UpSertSql =
        """
        insert into transacao (id, data, cliente_id, valor, descricao)
        values (@id, @data, @cliente_id, @valor, @descricao)
        on conflict (id) do update
        set data = excluded.data,
            cliente_id = excluded.cliente_id,
            valor = excluded.valor,
            descricao = excluded.descricao
        """;

    public static async Task ExecuteAsync(int count)
    {
        var rule = new Rule("[red]Inserindo dados iniciais na tabela[/]")
        {
            Justification = Justify.Left,
            Border = BoxBorder.Heavy,
        };
        AnsiConsole.Write(rule);
        var insertedItems = await InsertWithCopyUseCase.ExecuteAsync(count, 50_000);
        AnsiConsole.WriteLine();

        rule.Title = "[red]Executando teste de upsert[/]";
        AnsiConsole.Write(rule);

        // Deixar somente metade dos registros, e gerar novos para outra metade
        // a ideia é gerar 50% de insert, e 50% de update nesse teste
        var half = count / 2;
        var items = insertedItems.Take(half).ToList();
        items.ForEach(x => x.Descricao = $"(atualizado) {x.Descricao}".Truncate(40));
        var newItemsToInsert = TransacaoFactory.Generate(half);
        AnsiConsole.MarkupLine($"[gray]{newItemsToInsert.Count:N0} novos dados para inserts[/]");
        AnsiConsole.MarkupLine($"[gray]{items.Count:N0} com PK existente para updates[/]");
        items.AddRange(newItemsToInsert);

        // Início do processo com DB
        await using var conn = await DbFactory.CreateConnectionAsync();

        var watch = Stopwatch.StartNew();
        await using var transaction = await conn.BeginTransactionAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = UpSertSql;

        foreach (var item in items)
        {
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("id", item.Id);
            cmd.Parameters.AddWithValue("data", item.Data);
            cmd.Parameters.AddWithValue("cliente_id", item.ClienteId);
            cmd.Parameters.AddWithValue("valor", item.Valor);
            cmd.Parameters.AddWithValue("descricao", item.Descricao);
            await cmd.ExecuteNonQueryAsync();
        }

        await transaction.CommitAsync();
        watch.Stop();

        UseCaseExtensions.PrintStatistics(items.Count, watch.Elapsed, "inseridos ou atualizados");
    }
}