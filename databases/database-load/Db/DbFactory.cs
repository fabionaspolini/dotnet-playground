using Npgsql;

namespace database_load_playground.Db;

public static class DbFactory
{
    public static async Task<NpgsqlConnection> CreateConnectionAsync()
    {
        var conn = new NpgsqlConnection(Consts.ConnectionString);
        await conn.OpenAsync();
        return conn;
    }
}