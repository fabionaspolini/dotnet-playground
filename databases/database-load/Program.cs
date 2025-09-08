using database_load_playground;
using database_load_playground.Db;
using Microsoft.EntityFrameworkCore;

Console.WriteLine(".:: Database Load Playground ::.");

var optionsBuilder = new DbContextOptionsBuilder<LoadContext>();
optionsBuilder.UseNpgsql(Consts.ConnectionString);
optionsBuilder.LogTo(Console.WriteLine);

// Usar EF Migrations para criar banco de dados e tabelas
Console.WriteLine("Aplicando Migrations...");
using var conn = new LoadContext(optionsBuilder.Options);
conn.Database.Migrate();

Console.WriteLine("Fim");

namespace database_load_playground
{
    public static class Consts
    {
        public const string ConnectionString = "Server=localhost;Port=5432;Database=load-playground;User Id=postgres;Password=123456;";
    }
}