using System.Diagnostics;
using database_load_playground.Db;
using database_load_playground.Entities;
using NpgsqlTypes;

namespace database_load_playground.UseCases;

public static class BulkInsertWithCopyUseCase
{
    private const string Sql = 
        """
        COPY transacao (id, data, cliente_id, valor, descricao)
        FROM STDIN (FORMAT binary)
        """;
    public static async Task<List<Transacao>> ExecuteAsync(int count, int chunkSize = 0)
    {
        var items = TransacaoFactory.Generate(count);
        var chunk = items.Chunk(chunkSize > 0 ? chunkSize : items.Count);

        await using var conn = await DbFactory.CreateConnectionAsync();
        // var transaction = await conn.BeginTransactionAsync();
        
        var watch = Stopwatch.StartNew();
        foreach (var part in chunk)
        {
            await using var writer = await conn.BeginBinaryImportAsync(Sql);
            foreach (var item in part)
            {
                await writer.StartRowAsync();
                await writer.WriteAsync(item.Id, NpgsqlDbType.Uuid);
                await writer.WriteAsync(item.Data, NpgsqlDbType.Timestamp);
                await writer.WriteAsync(item.ClienteId, NpgsqlDbType.Uuid);
                await writer.WriteAsync(item.Valor, NpgsqlDbType.Numeric);
                await writer.WriteAsync(item.Descricao, NpgsqlDbType.Varchar);
            }
            await writer.CompleteAsync();
        }

        // await transaction.CommitAsync();

        watch.Stop();
        
        UseCaseExtensions.PrintStatistics(items.Count, watch.Elapsed);
        return items;
    }
}