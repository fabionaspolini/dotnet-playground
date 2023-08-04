using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Npgsql;
using System.Data;
using System.Diagnostics;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(".:: Full Databases Playground - Benchmark ::.");
        BenchmarkRunner.Run<DatabaseBenchmark>();
    }
}

public static class TestParams
{
    public static readonly TimeSpan TotalTestTime = TimeSpan.FromSeconds(10);
    //public const int Threads = 10;
    public static readonly string AppName = AppDomain.CurrentDomain.FriendlyName;
}

public enum DatabaseEngine
{
    MariaDb,
    MySql,
    PostgreSql,
    SqlServer
}

[ShortRunJob(RuntimeMoniker.Net70), AllStatisticsColumn, RPlotExporter]
public class DatabaseBenchmark
{
    private IDbConnection _connection = default!;

    [Params(DatabaseEngine.MariaDb, DatabaseEngine.MySql, DatabaseEngine.PostgreSql, DatabaseEngine.SqlServer)]
    public DatabaseEngine Engine;

    [GlobalSetup]
    public void Setup()
    {
        _connection = CreateConnection();
        _connection.Open();
    }

    [Benchmark]
    public void SyncQuery()
    {
        _connection.QueryFirst<Pessoa>("select * from pessoa where id = @id", new { id = 1 });
    }

    [Benchmark]
    public async Task AsyncQuery()
    {
        await _connection.QueryFirstAsync<Pessoa>("select * from pessoa where id = @id", new { id = 1 });
    }

    private IDbConnection CreateConnection() => Engine switch
    {
        DatabaseEngine.MariaDb => new MySqlConnection($"Server=127.0.0.1;Port=3307;Database=teste;Uid=root;Pwd=admin;ApplicationName={TestParams.AppName};"),
        DatabaseEngine.MySql => new MySqlConnection($"Server=127.0.0.1;Port=3306;Database=teste;Uid=root;Pwd=admin;ApplicationName={TestParams.AppName};"),
        DatabaseEngine.PostgreSql => new NpgsqlConnection($"Server=127.0.0.1;Port=5432;Database=teste;User Id=postgres;Password=123456;MaxPoolSize=200;ApplicationName={TestParams.AppName};"),
        DatabaseEngine.SqlServer => new SqlConnection($"Server=127.0.0.1;Database=teste;User Id=sa;Password=Pass123456;Application Name={TestParams.AppName};TrustServerCertificate=True;"),
        _ => throw new NotSupportedException($"Database engine não suportado: {Engine}")
    };
}

record Pessoa(int Id, string Nome);