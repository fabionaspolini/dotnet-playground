using System.Diagnostics;
using database_load_playground.Db;
using database_load_playground.Entities;
using Maestria.Extensions;
using Spectre.Console;

namespace database_load_playground.UseCases;

public static class BulkUpsertUseCase
{
    private const string CreateTemporaryTableSql =
        """
        create temp table transacao_temp
        on commit drop
        as table transacao with no data
        """;
    
    private const string UpsertSql =
        """
        insert into transacao
        select * from transacao_temp
        on conflict (id) do update
        set
        	"data" = excluded."data",
        	cliente_id = excluded.cliente_id,
        	valor = excluded.valor,
        	descricao = excluded.descricao;
        """;

    public static async Task ExecuteAsync(int count)
    {
        // var items = TransacaoFactory.Generate(count);
        var rule = new Rule("[red]Inserindo dados iniciais na tabela[/]")
        {
            Justification = Justify.Left,
            Border = BoxBorder.Heavy,
        };
        AnsiConsole.Write(rule);
        var items = await InsertWithCopyUseCase.ExecuteAsync(count, 50_000);
        AnsiConsole.WriteLine();

        rule.Title = "[red]Executando teste de upsert[/]";
        AnsiConsole.Write(rule);

        // Deixar somente metade dos registros, e gerar novos para outra metade
        // a ideia é gerar 50% de insert, e 50% de update nesse teste
        var half = count / 2;
        items.RemoveRange(0, half);
        items.ForEach(x => x.Descricao = $"(atualizado) {x.Descricao}".Truncate(40));
        var newItems = TransacaoFactory.Generate(half);
        items.AddRange(newItems);
        AnsiConsole.MarkupLine($"[gray]{newItems.Count:N0} novos dados para inserts[/]");
        AnsiConsole.MarkupLine($"[gray]{count - half:N0} com PK existente para updates[/]");

        // Executar
        await using var conn = await DbFactory.CreateConnectionAsync();

        var watch = Stopwatch.StartNew();
        await using var transaction = await conn.BeginTransactionAsync();
        await using var cmd = conn.CreateCommand();

        // Criar e inserir dados na tabela temporária
        cmd.CommandText = CreateTemporaryTableSql;
        await cmd.ExecuteNonQueryAsync();
        await InternalBulkInsertUseCase.ExecuteAsync(conn, items, "transacao_temp", partialMessage: "inseridos na tabela temporária");
        
        // Insert ou update na tabela definitiva
        cmd.CommandText = UpsertSql;
        await cmd.ExecuteNonQueryAsync();

        // Commit da transação apaga automaticamente a tabela temporária devido a cláusula "on commit drop"
        await transaction.CommitAsync();
        watch.Stop();

        UseCaseExtensions.PrintStatistics(items.Count, watch.Elapsed, "inseridos ou atualizados");
    }
}