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

| Use case                        | Resultado      |
|---------------------------------|----------------|
| 1) Insert without transaction   | 6 segundos     |
| 2) Insert with transaction      | 1,28 segundos  |
| 3) Insert with multiples values | 0,091 segundos |