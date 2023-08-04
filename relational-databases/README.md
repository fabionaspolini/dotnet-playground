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
|  SyncQuery | PostgreSql | 297.9 μs |  64.96 μs | 3.56 μs | 2.06 μs | 295.8 μs | 295.9 μs | 296.0 μs | 299.0 μs | 302.0 μs | 3,356.6 |
|  SyncQuery |    MariaDb | 338.4 μs |  55.19 μs | 3.03 μs | 1.75 μs | 335.5 μs | 336.8 μs | 338.1 μs | 339.8 μs | 341.5 μs | 2,955.1 |
|  SyncQuery |      MySql | 380.4 μs |  93.26 μs | 5.11 μs | 2.95 μs | 374.5 μs | 378.7 μs | 382.9 μs | 383.3 μs | 383.7 μs | 2,629.1 |
|  SyncQuery |  SqlServer | 389.3 μs |  35.64 μs | 1.95 μs | 1.13 μs | 387.8 μs | 388.2 μs | 388.7 μs | 390.1 μs | 391.5 μs | 2,568.6 |
|            |            |          |           |         |         |          |          |          |          |          |         |
| AsyncQuery | PostgreSql | 340.3 μs |  36.56 μs | 2.00 μs | 1.16 μs | 338.9 μs | 339.1 μs | 339.3 μs | 341.0 μs | 342.6 μs | 2,938.7 |
| AsyncQuery |    MariaDb | 390.8 μs |  58.68 μs | 3.22 μs | 1.86 μs | 387.1 μs | 389.8 μs | 392.5 μs | 392.7 μs | 392.8 μs | 2,558.9 |
| AsyncQuery |      MySql | 430.0 μs | 120.04 μs | 6.58 μs | 3.80 μs | 424.1 μs | 426.5 μs | 428.8 μs | 433.0 μs | 437.1 μs | 2,325.5 |
| AsyncQuery |  SqlServer | 449.3 μs |  49.34 μs | 2.70 μs | 1.56 μs | 446.6 μs | 447.9 μs | 449.1 μs | 450.6 μs | 452.0 μs | 2,225.8 |


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
