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
- Driver MySql.Data apresenta erros com alta concorrência multi thread

### Resultados full-databases-benchmark com BenchmarkDotNet

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
|  **SyncQuery** |    **MariaDb** | **333.5 μs** |  **13.99 μs** | **0.77 μs** | **0.44 μs** | **332.8 μs** | **333.1 μs** | **333.4 μs** | **333.8 μs** | **334.3 μs** | **2,998.6** |
| AsyncQuery |    MariaDb | 397.1 μs |  52.49 μs | 2.88 μs | 1.66 μs | 393.9 μs | 396.0 μs | 398.2 μs | 398.8 μs | 399.3 μs | 2,518.1 |
|  **SyncQuery** |      **MySql** | **375.4 μs** |  **35.07 μs** | **1.92 μs** | **1.11 μs** | **373.4 μs** | **374.5 μs** | **375.6 μs** | **376.4 μs** | **377.3 μs** | **2,663.6** |
| AsyncQuery |      MySql | 432.3 μs | 102.50 μs | 5.62 μs | 3.24 μs | 428.8 μs | 429.0 μs | 429.2 μs | 434.0 μs | 438.7 μs | 2,313.4 |
|  **SyncQuery** | **PostgreSql** | **310.7 μs** |  **17.15 μs** | **0.94 μs** | **0.54 μs** | **309.9 μs** | **310.2 μs** | **310.6 μs** | **311.2 μs** | **311.8 μs** | **3,218.1** |
| AsyncQuery | PostgreSql | 344.4 μs |  36.41 μs | 2.00 μs | 1.15 μs | 342.9 μs | 343.3 μs | 343.7 μs | 345.2 μs | 346.7 μs | 2,903.2 |
|  **SyncQuery** |  **SqlServer** | **390.1 μs** |  **39.59 μs** | **2.17 μs** | **1.25 μs** | **388.2 μs** | **389.0 μs** | **389.7 μs** | **391.1 μs** | **392.5 μs** | **2,563.2** |
| AsyncQuery |  SqlServer | 459.8 μs | 138.12 μs | 7.57 μs | 4.37 μs | 455.0 μs | 455.4 μs | 455.8 μs | 462.2 μs | 468.5 μs | 2,175.0 |


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
