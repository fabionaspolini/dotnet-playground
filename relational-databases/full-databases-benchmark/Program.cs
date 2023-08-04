using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using Dapper;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using Npgsql;
using System.Data;

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

//[DryJob]
//[SimpleJob(RuntimeMoniker.Net70), AllStatisticsColumn, RPlotExporter]
//[SimpleJob(runtimeMoniker: RuntimeMoniker.Net70, runStrategy: RunStrategy.Monitoring, launchCount: 1, warmupCount: 2, iterationCount: 1_000), AllStatisticsColumn, RPlotExporter]
[ShortRunJob(RuntimeMoniker.Net70), AllStatisticsColumn, RPlotExporter]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByMethod)]
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