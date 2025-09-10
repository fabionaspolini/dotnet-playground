using System.Diagnostics;
using database_load_playground.Db;
using database_load_playground.Entities;
using EFCore.BulkExtensions;
using Maestria.Extensions;
using Spectre.Console;

namespace database_load_playground.UseCases;

public static class BulkUpsertEntityFrameworkUseCase
{
    public static async Task ExecuteAsync(int count, int chunkSize)
    {
        // var items = TransacaoFactory.Generate(count);
        var rule = new Rule("[red]Inserindo dados iniciais na tabela[/]")
        {
            Justification = Justify.Left,
            Border = BoxBorder.Heavy,
        };
        AnsiConsole.Write(rule);
        var insertedItems = await BulkInsertWithCopyUseCase.ExecuteAsync(count, chunkSize);
        AnsiConsole.WriteLine();

        rule.Title = "[red]Executando teste de upsert[/]";
        AnsiConsole.Write(rule);

        // Deixar somente metade dos registros, e gerar novos para outra metade
        // a ideia é gerar 50% de insert, e 50% de update nesse teste
        var half = count / 2;
        var items = insertedItems.Take(half).ToList();
        items.ForEach(x => x.Descricao = $"(atualizado) {x.Descricao}".Truncate(40));
        var newItemsToInsert = TransacaoFactory.Generate(half);
        AnsiConsole.MarkupLine($"[gray]{newItemsToInsert.Count:N0} novos dados para inserts[/]");
        AnsiConsole.MarkupLine($"[gray]{items.Count:N0} com PK existente para updates[/]");
        items.AddRange(newItemsToInsert);

        // Início do processo de bulk upsert
        await using var dbContext = DbFactory.CreateEfContext();
        
        var watch = Stopwatch.StartNew();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        await dbContext.BulkInsertOrUpdateAsync(items, opts =>
        {
            opts.BatchSize = chunkSize > 0 ? chunkSize : 50_000;
            opts.UniqueTableNameTempDb = false;
            opts.UseTempDB = true;
        });

        await transaction.CommitAsync();
        watch.Stop();

        UseCaseExtensions.PrintStatistics(items.Count, watch.Elapsed, "inseridos ou atualizados na tabela oficial");
    }
}