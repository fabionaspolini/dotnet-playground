using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
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
        //BenchmarkRunner.Run<DatabaseBenchmark>();
        BenchmarkRunner.Run<MultiThreadDatabaseBenchmark>();
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

record Pessoa(int Id, string Nome);

//[DryJob]
//[SimpleJob(RuntimeMoniker.Net70), AllStatisticsColumn, RPlotExporter]
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

/// <summary>
/// Simulação multi thread não é fiel a um burst test. BenchmarkDotNet não possui feature para multi threads adequadas. <br/>
/// Estão sendo criadas 10 threads por iteração do BenchmarkDotNet, e cada thread realiza conexão com o DB + 1000 consultas. <br/>
/// Portando, o resultado final de operações por segundo deve ser multiplicado x 10 x 1000.
/// </summary>
//[DryJob]
[SimpleJob(RuntimeMoniker.Net70, launchCount: 1, warmupCount: 2, iterationCount: 10), AllStatisticsColumn, RPlotExporter]
//[ShortRunJob(RuntimeMoniker.Net70), AllStatisticsColumn, RPlotExporter]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByMethod)]
public class MultiThreadDatabaseBenchmark
{
    [Params(DatabaseEngine.MariaDb, DatabaseEngine.MySql, DatabaseEngine.PostgreSql, DatabaseEngine.SqlServer)]
    //[Params(DatabaseEngine.PostgreSql)]
    public DatabaseEngine Engine;

    [Params(10)]
    public int Threads;

    [Params(1_000)]
    public int ConsultasPorThread;

    [Benchmark]
    public void SyncQuery()
    {
        var tasks = new Task[Threads];
        for (int i = 0; i < Threads; i++)
            tasks[i] = Task.Run(() =>
            {
                using var conn = CreateConnection();
                conn.Open();
                for (int j = 0; j < ConsultasPorThread; j++)
                    conn.QueryFirst<Pessoa>("select * from pessoa where id = @id", new { id = 1 });
            });
        Task.WaitAll(tasks);
    }

    [Benchmark]
    public async Task AsyncQuery()
    {
        var tasks = new Task[Threads];
        for (int i = 0; i < Threads; i++)
            tasks[i] = Task.Run(async () =>
            {
                using var conn = CreateConnection();
                conn.Open();
                for (int j = 0; j < ConsultasPorThread; j++)
                    await conn.QueryFirstAsync<Pessoa>("select * from pessoa where id = @id", new { id = 1 });
            });

        for (int i = 0; i < Threads; i++)
            await tasks[i];
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