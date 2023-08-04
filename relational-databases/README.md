# Relational Databases

## Benchmark

### Resultados single thread

Setup:

- Framework para consultas: Dapper
- Tempo: 10 segundos

| Engine        | 1 Thread                        | 10 Threads                      | 100 Threads                       |
|---------------|---------------------------------|---------------------------------|-----------------------------------|
| PostgreSQL 14 | Total: 26.202 - 2.620,2 op/sec  | Total: 27.711 - 2.771,1 op/sec  | Total: 30.744 - 3.074,4 op/sec123    |
| MySQL 8       | Total: 5.381 - 538,1 op/sec     |

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
