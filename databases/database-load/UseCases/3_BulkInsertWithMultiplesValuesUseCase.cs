using System.Diagnostics;
using System.Text;
using database_load_playground.Db;
using database_load_playground.Entities;

namespace database_load_playground.UseCases;

public static class BulkInsertWithMultiplesValuesUseCase
{
    public static async Task ExecuteAsync(int count, int chunkSize = 0)
    {
        var items = TransacaoFactory.Generate(count);
        var chunk = items.Chunk(chunkSize > 0 ? chunkSize : items.Count);

        await using var conn = await DbFactory.CreateConnectionAsync();
        
        var watch = Stopwatch.StartNew();
        foreach (var part in chunk)
        {
            var sql = new StringBuilder();
            sql.AppendLine("insert into transacao (id, data, cliente_id, valor, descricao)");
            sql.AppendLine("values");
            foreach (var item in part)
                sql.AppendLine($"(" +
                               $"{item.Id.ToSqlValue()}, " +
                               $"{item.Data.ToSqlValue()}, " +
                               $"{item.ClienteId.ToSqlValue()}, " +
                               $"{item.Valor.ToSqlValue()}, " +
                               $"{item.Descricao.ToSqlValue()}" +
                               $"),");
            var lastCommaIndexOf = sql.ToString().LastIndexOf(',');
            sql.Remove(lastCommaIndexOf, sql.Length - lastCommaIndexOf);
            
            await using var transaction = await conn.BeginTransactionAsync();
            await using var cmd = conn.CreateCommand();

            cmd.CommandText = sql.ToString();
            await cmd.ExecuteNonQueryAsync();
            await transaction.CommitAsync();
        }

        watch.Stop();
        
        UseCaseExtensions.PrintStatistics(items.Count, watch.Elapsed);
    }
}