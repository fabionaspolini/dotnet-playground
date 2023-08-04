# Relational Databases

## Benchmark

### Resultados single thread

Setup:

- Framework para consultas: Dapper
- Tempo: 10 segundos

| Engine        | Driver         | 1 Thread                 | 10 Threads               | 100 Threads                |
|---------------|----------------|--------------------------|--------------------------|----------------------------|
| PostgreSQL 14 | Npgsql         | 25.787 - 2.578,7 op/sec  | 28.209 - 2.820,9 op/sec  | 29.410 - 2.941,0 op/sec    |
| MySQL 8       | MySqlConnector | 13.018 - 1.301,8 op/sec  | 12.756 - 1.275,6 op/sec  | 13.013 - 1.301,3 op/sec    |
| MySQL 8       | MySql.Data     | 5.381 - 538,1 op/sec     | Instável com burst test  | Instável com burst test    |

- [Npgsql](https://www.npgsql.org/doc/connection-string-parameters.html)
- [MySqlConnector Driver](https://mysqlconnector.net/connection-options/)

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
