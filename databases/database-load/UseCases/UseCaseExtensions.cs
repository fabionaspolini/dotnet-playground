using System.Globalization;
using Spectre.Console;

namespace database_load_playground.UseCases;

public static class UseCaseExtensions
{
    public static void PrintStatistics(int records, TimeSpan elapsed, string partialMessage = "inseridos") =>
        AnsiConsole.MarkupLine($"[green]{records:N0}[/] registros {partialMessage} em [green]{elapsed}[/]");
    
    public static string ToSqlValue(this string value) => $"'{value}'";
    public static string ToSqlValue(this Guid value) => value.ToString().ToSqlValue();
    public static string ToSqlValue(this DateTime value) => value.ToString(CultureInfo.InvariantCulture).ToSqlValue();
    public static string ToSqlValue(this decimal value) => value.ToString(CultureInfo.InvariantCulture);
}