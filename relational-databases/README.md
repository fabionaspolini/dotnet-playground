# Relational Databases

## Benchmark

Setup:

- Framework para consultas: Dapper.
- Tempo: 10 segundos.
- Teste simples rodando 10 segundos de consultas. Resultado no formato (total-de-queries) - (operações-por-segundo).
- DBs executando em container docker. Limitados a 6 cores CPU x 12 gb RAM
- Host: 12 cores CPU x 32 gb RAM (AMD Ryzen 5 5600G x ASUS TUF GAMING B550M-PLUS x T-FORCE VULCAN DDR4 3200MHZ)

- [Npgsql](https://www.npgsql.org/doc/connection-string-parameters.html)
- [MySqlConnector Driver](https://mysqlconnector.net/connection-options/)

### Resultados isolados com Burst Test

Sync para usar todo o poder de processamento no Burst Test, mas numa operação real de produção,
com Async é possível aproveitar melhor os recursos de hardware em alta concorrência.

Consumo de CPU e RAM no async é menor.

**sync queries**

| Engine         | Driver                   | 1 Thread                 | 10 Threads                 |
|----------------|--------------------------|--------------------------|----------------------------|
| PostgreSQL 14  | Npgsql                   | 32.791 - 3.279,1 op/sec  | 217.905 - 21.790,5 op/sec  |
| MariaDB 11     | MySqlConnector           | 29.795 - 2.979,5 op/sec  | 167.712 - 16.771,2 op/sec  |
| MySQL 8        | MySqlConnector           | 26.397 - 2.639,7 op/sec  | 138.956 - 13.895,6 op/sec  |
| SqlServer 2019 | Microsoft.Data.SqlClient | 24.582 - 2.458,2 op/sec  | 150.156 - 15.015,6 op/sec  |

**async queries**

| Engine         | Driver                   | 1 Thread                 | 10 Threads                 |
|----------------|--------------------------|--------------------------|----------------------------|
| PostgreSQL 14  | Npgsql                   | 25.787 - 2.578,7 op/sec  | 28.209 - 2.820,9 op/sec    |
| MariaDB 11     | MySqlConnector           | 16.323 - 1.632,3 op/sec  | 15.214 - 1.521,4 op/sec    |
| MySQL 8        | MySqlConnector           | 13.018 - 1.301,8 op/sec  | 12.756 - 1.275,6 op/sec    |
| MySQL 8        | MySql.Data               | 5.381 - 538,1 op/sec     | Instável com burst test    |
| SqlServer 2019 | Microsoft.Data.SqlClient | 21.577 - 2.157,7 op/sec  | 90.200 - 9.020,0 op/sec    |

- Driver SQL Server enviou muito mais queries para o DB no modo assincrono do que os demais.
- SQL Server na versão 2019 Developer for Linux
- Driver MySql.Data apresenta erros com alta concorrência multi thread.
    <details>
      <summary>Show exception</summary>
  
      (Operations that change non-concurrent collections must have exclusive access.
       A concurrent update was performed on this collection and corrupted its state.
       The collection's state is no longer correct.)
      (An item with the same key has already been added. Key: server=127.0.0.1;port=3306;database=teste;user id=root;password=admin)
      ---> System.InvalidOperationException: Operations that change non-concurrent collections must have exclusive access. A concurrent update was performed on this collection and corrupted its state. The collection's state is no longer correct.
      at System.Collections.Generic.Dictionary`2.TryInsert(TKey key, TValue value, InsertionBehavior behavior)
         at System.Collections.Generic.Dictionary`2.Add(TKey key, TValue value)
      at MySql.Data.MySqlClient.MySqlPoolManager.GetPoolAsync(MySqlConnectionStringBuilder settings, Boolean execAsync, CancellationToken cancellationToken)
      at MySql.Data.MySqlClient.MySqlConnection.OpenAsync(Boolean execAsync, CancellationToken cancellationToken)
      at Program.<>c__DisplayClass0_1.<<<Main>$>b__3>d.MoveNext() in C:\Sources\samples\dotnet-playground\relational-databases\mysql-benchmark\Program.cs:line 35
    </details>

### Resultados full-databases-benchmark com BenchmarkDotNet

**DatabaseBenchmark - Single Thread**

```

BenchmarkDotNet v0.13.6, Windows 11 (10.0.22621.1992/22H2/2022Update/SunValley2)
AMD Ryzen 5 5600G with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 7.0.304
  [Host]            : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2 [AttachedDebugger]
  ShortRun-.NET 7.0 : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2

Job=ShortRun-.NET 7.0  Runtime=.NET 7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|     Method |     Engine |     Mean |     Error |  StdDev |  StdErr |      Min |       Q1 |   Median |       Q3 |      Max |    Op/s |
|----------- |----------- |---------:|----------:|--------:|--------:|---------:|---------:|---------:|---------:|---------:|--------:|
|  SyncQuery | PostgreSql | 297.9 μs |  64.96 μs | 3.56 μs | 2.06 μs | 295.8 μs | 295.9 μs | 296.0 μs | 299.0 μs | 302.0 μs | 3,356.6 |
|  SyncQuery |    MariaDb | 338.4 μs |  55.19 μs | 3.03 μs | 1.75 μs | 335.5 μs | 336.8 μs | 338.1 μs | 339.8 μs | 341.5 μs | 2,955.1 |
|  SyncQuery |      MySql | 380.4 μs |  93.26 μs | 5.11 μs | 2.95 μs | 374.5 μs | 378.7 μs | 382.9 μs | 383.3 μs | 383.7 μs | 2,629.1 |
|  SyncQuery |  SqlServer | 389.3 μs |  35.64 μs | 1.95 μs | 1.13 μs | 387.8 μs | 388.2 μs | 388.7 μs | 390.1 μs | 391.5 μs | 2,568.6 |
|            |            |          |           |         |         |          |          |          |          |          |         |
| AsyncQuery | PostgreSql | 340.3 μs |  36.56 μs | 2.00 μs | 1.16 μs | 338.9 μs | 339.1 μs | 339.3 μs | 341.0 μs | 342.6 μs | 2,938.7 |
| AsyncQuery |    MariaDb | 390.8 μs |  58.68 μs | 3.22 μs | 1.86 μs | 387.1 μs | 389.8 μs | 392.5 μs | 392.7 μs | 392.8 μs | 2,558.9 |
| AsyncQuery |      MySql | 430.0 μs | 120.04 μs | 6.58 μs | 3.80 μs | 424.1 μs | 426.5 μs | 428.8 μs | 433.0 μs | 437.1 μs | 2,325.5 |
| AsyncQuery |  SqlServer | 449.3 μs |  49.34 μs | 2.70 μs | 1.56 μs | 446.6 μs | 447.9 μs | 449.1 μs | 450.6 μs | 452.0 μs | 2,225.8 |


**MultiThreadDatabaseBenchmark - Multi Thread**

> Simulação multi thread não é fiel a um burst test. BenchmarkDotNet não possui feature para multi threads adequadas.  
> Estão sendo criadas 10 threads por iteração do BenchmarkDotNet, e cada thread realiza conexão com o DB + 1000 consultas.  
> Portando, o resultado final de operações por segundo deve ser multiplicado x 10 x 1000.  
>
> O número real de operações por segundo é na faixa de 14.000 a 22.000 aproximadamente (Formatação de decimal americada com ".").

```

BenchmarkDotNet v0.13.6, Windows 11 (10.0.22621.1992/22H2/2022Update/SunValley2)
AMD Ryzen 5 5600G with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 7.0.304
  [Host]     : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2 [AttachedDebugger]
  Job-UOMVIQ : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2

Runtime=.NET 7.0  IterationCount=10  LaunchCount=1  
WarmupCount=2  

```
|     Method |     Engine | Threads | ConsultasPorThread |     Mean |    Error |   StdDev |  StdErr |      Min |       Q1 |   Median |       Q3 |      Max |  Op/s |
|----------- |----------- |-------- |------------------- |---------:|---------:|---------:|--------:|---------:|---------:|---------:|---------:|---------:|------:|
|  SyncQuery | PostgreSql |      10 |               1000 | 451.1 ms |  7.18 ms |  4.75 ms | 1.50 ms | 444.0 ms | 447.8 ms | 452.0 ms | 454.1 ms | 458.9 ms | 2.217 |
|  SyncQuery |    MariaDb |      10 |               1000 | 588.8 ms |  9.77 ms |  5.81 ms | 1.94 ms | 578.6 ms | 585.3 ms | 588.9 ms | 590.5 ms | 597.0 ms | 1.698 |
|  SyncQuery |  SqlServer |      10 |               1000 | 668.6 ms | 25.97 ms | 15.45 ms | 5.15 ms | 656.2 ms | 658.1 ms | 663.9 ms | 670.8 ms | 703.5 ms | 1.496 |
|  SyncQuery |      MySql |      10 |               1000 | 695.4 ms |  7.78 ms |  5.15 ms | 1.63 ms | 690.6 ms | 691.0 ms | 693.5 ms | 698.4 ms | 703.6 ms | 1.438 |
|            |            |         |                    |          |          |          |         |          |          |          |          |          |       |
| AsyncQuery | PostgreSql |      10 |               1000 | 530.3 ms |  8.27 ms |  4.33 ms | 1.53 ms | 524.0 ms | 526.5 ms | 531.9 ms | 532.8 ms | 536.4 ms | 1.886 |
| AsyncQuery |  SqlServer |      10 |               1000 | 750.1 ms | 39.48 ms | 23.49 ms | 7.83 ms | 732.9 ms | 735.7 ms | 741.6 ms | 743.0 ms | 803.8 ms | 1.333 |
| AsyncQuery |    MariaDb |      10 |               1000 | 754.3 ms | 13.68 ms |  8.14 ms | 2.71 ms | 747.7 ms | 748.4 ms | 751.9 ms | 754.0 ms | 769.7 ms | 1.326 |
| AsyncQuery |      MySql |      10 |               1000 | 880.3 ms | 11.92 ms |  6.23 ms | 2.20 ms | 872.7 ms | 875.0 ms | 879.1 ms | 886.6 ms | 888.1 ms | 1.136 |


### Create database script

```bash
create database teste;

CREATE TABLE pessoa (
    id int4 NOT NULL,
    nome varchar(60) NOT NULL,
    CONSTRAINT pessoa_pk PRIMARY KEY (id)
);

INSERT INTO pessoa (id,nome) VALUES
     (1,'Fulano'),
     (2,'Beltrano');

select * from pessoa p;
```
