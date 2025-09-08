## Instruções para montar ambiente de execução

1. Instalar SDK .NET 9.0: <https://dotnet.microsoft.com/download/dotnet/9.0>
2. Instalar ferramentas CLI de migrations do EF Core

```bash
dotnet tool install --global dotnet-ef
```

3. Subir instância postgres para teste

```bash
docker run -d --name postgres -e POSTGRES_PASSWORD=123456 -p 5432:5432 postgres:17
```

4. Executar aplicação

```bash
dotnet run
```

Será criado automaticamente o banco de dados "ef-core-playground".

## Migrations

Instruções sobre manipulação da base de dados com migrations.
Use caso tenha alterado a estrutura de dados do exemplo.

Adicionar nova migration.

```bash
dotnet ef migrations add Inicio
```

Atualizar base de dados.

```bash
dotnet ef database update
```

Remover migrations e resetar base de dados.

```bash
dotnet ef database update 0
dotnet ef migrations remove
```

## Resultado

| Use case                                                          | Resultado                                                                                   |
|-------------------------------------------------------------------|---------------------------------------------------------------------------------------------|
| 1) Insert without transaction -> 10 mil                           | 6 segundos                                                                                  |
| 2) Insert with transaction -> 10 mil                              | 1,28 segundos                                                                               |
| 3.1) Insert with multiples values -> 10 mil                       | 0,11 segundos                                                                               |
| 3.2) Insert with multiples values -> 1 milhão                     | 9,6 segundos e 100% de CPU do DB. Começa a ficar péssimo para ambiente de alta concorrência |
| 3.3) Insert with multiples values -> 1 milhão em pacotes de 5 mil | 9 seg, mas ainda com muito CPU do DB                                                        |
| 4) Parallel insert (WTF!)                                         | Algoritmo ruim não escala, não adianta paralelizar!                                         |
| 5.1) Insert with copy -> 1 milhão                                 | 2,4 segundos, há um pico de CPU, mas afeta menos o ambiente devido a velocidade de inserção |
| 5.2) Insert with copy -> 1 milhão em pacotes de 5 mil             | 2,8 segundos, com picos menores de CPU                                                      |
| 5.3) Insert with copy -> 10 milhões                               | 55 segundos, CPU de 27% a 85%                                                               |
| 5.4) Insert with copy -> 10 milhões em pacotes de 5 mil           | 48 segundos, CPU de 21% a 85%                                                               |
| 5.5) Insert with copy -> 10 milhões em pacotes de 50 mil          | 29 segundos, CPU em 77%                                                                     |
| 10.1) Insert or update -> 10 mil                                  | 8 segundos                                                                                  |
| 10.2) Insert or update -> 50 mil                                  | 40 segundos, CPU em 30%. Processo lento, se paralelizar topa DB                             |


Notas:
- 1 e 2: Também utilizam muito CPU, mas devido a lentidão de envio do comando pro DB, não chega a bater 100%.
Porém, não significa que é bom, o tempo total do usuário é muito maior, e se for algo em paralelo, conseguirá bater 100% de CPU.
- 3.x: Todos em transaction.
- 5.x:
  - Atenção com formato de dados, tentar escrever um número num campo string pode ocorrer exception, ou no pior dos casos, causar corrupção de dados.
  - De preferência por utilizar o overload que informa o tipo de dados.
  - Mais informações em https://www.npgsql.org/doc/copy.html