using System.Diagnostics;
using database_load_playground.Db;
using database_load_playground.Entities;
using Maestria.Extensions;
using Spectre.Console;

namespace database_load_playground.UseCases;

public static class InsertOrUpdateUseCase
{
    private const string InsertSql =
        """
        insert into transacao (id, data, cliente_id, valor, descricao)
        values (@id, @data, @cliente_id, @valor, @descricao)
        """;
    
    private const string UpdateSql =
        """
        update transacao
        set data = @data,
            cliente_id = @cliente_id,
            valor = @valor,
            descricao = @descricao
        where id = @id
        """;
    
    private const string CheckExistsSql =
        """
        select 1
        from transacao
        where id = @id
        """;
    
    public static async Task ExecuteAsync(int count)
    {
        var rule = new Rule("[red]Inserindo dados iniciais na tabela[/]")
        {
            Justification = Justify.Left,
            Border = BoxBorder.Heavy,
        };
        AnsiConsole.Write(rule);
        var insertedItems = await BulkInsertWithCopyUseCase.ExecuteAsync(count, 50_000);
        AnsiConsole.WriteLine();

        rule.Title = "[red]Executando teste de insert or update[/]";
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
        
        foreach (var item in items)
        {
            // Check exists
            cmd.CommandText = CheckExistsSql;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("id", item.Id);
            var exists = await cmd.ExecuteScalarAsync();
            
            // Insert or update
            cmd.CommandText = exists != null ? UpdateSql : InsertSql;
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