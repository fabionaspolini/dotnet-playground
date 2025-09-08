using System.Diagnostics;
using database_load_playground.Db;
using database_load_playground.Entities;

namespace database_load_playground.UseCases;

public static class InsertWithTransactionUseCase
{
    private const string Sql =
        """
        insert into transacao (id, cliente_id, valor, descricao)
        values (@id, @cliente_id, @valor, @descricao)
        """;
    public static async Task ExecuteAsync(int count)
    {
        var items = TransacaoFactory.Generate(count);
        
        await using var conn = await DbFactory.CreateConnectionAsync();
        
        var watch = Stopwatch.StartNew();
        await using var transaction = await conn.BeginTransactionAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = Sql;
        
        foreach (var item in items)
        {
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("id", item.Id);
            cmd.Parameters.AddWithValue("cliente_id", item.ClienteId);
            cmd.Parameters.AddWithValue("valor", item.Valor);
            cmd.Parameters.AddWithValue("descricao", item.Descricao);
            await cmd.ExecuteNonQueryAsync();
        }

        await transaction.CommitAsync();
        watch.Stop();
        
        UseCaseExtensions.PrintStatistics(items.Count, watch.Elapsed);
    }
}