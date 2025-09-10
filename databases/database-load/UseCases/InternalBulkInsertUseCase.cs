using System.Diagnostics;
using System.Text;
using database_load_playground.Db;
using database_load_playground.Entities;
using Maestria.Extensions;
using Npgsql;
using NpgsqlTypes;

namespace database_load_playground.UseCases;

public static class InternalBulkInsertUseCase
{
    private const string Sql = 
        """
        COPY {0} (id, data, cliente_id, valor, descricao)
        FROM STDIN (FORMAT binary)
        """;
    
    public static async Task ExecuteAsync(
        NpgsqlConnection conn,
        List<Transacao> items,
        string tableName = "transacao",
        int chunkSize = 0,
        string partialMessage = "inseridos")
    {
        var chunk = items.Chunk(chunkSize > 0 ? chunkSize : items.Count);
        var effectiveSql = string.Format(Sql, tableName);
        
        var watch = Stopwatch.StartNew();
        foreach (var part in chunk)
        {
            await using var writer = await conn.BeginBinaryImportAsync(effectiveSql);
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

        watch.Stop();
        
        UseCaseExtensions.PrintStatistics(items.Count, watch.Elapsed, partialMessage);
    }
}