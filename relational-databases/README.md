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

### Resultados

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
