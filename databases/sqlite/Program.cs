using Microsoft.Data.Sqlite;

Console.WriteLine(".:: SQLite Playground ::.");

var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
var connStrBuilder = new SqliteConnectionStringBuilder
{
    DataSource = Path.Combine(currentDirectory, "..", "..", "..", "sqlite-sample.db"),
    Mode = SqliteOpenMode.ReadWriteCreate,
    Cache = SqliteCacheMode.Shared,
};
// Data Source=mydb.db;

Console.WriteLine("ConnectionString:" + connStrBuilder.ConnectionString);

await using var conn = new SqliteConnection(connStrBuilder.ConnectionString);
await conn.OpenAsync();
await using var cmd = conn.CreateCommand();
cmd.CommandText = "select * from pessoa";
await using var reader = await cmd.ExecuteReaderAsync();

Console.WriteLine();
Console.WriteLine("Id | Nome");
while (reader.Read()) 
    Console.WriteLine($"{reader.GetValue(0), 2} | {reader.GetValue(1)}");

Console.WriteLine();
Console.WriteLine("Fim");

// conn.CreateCommand()