using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace database_load_playground.Db;

/// <summary>
/// Fábrica para suportar CLI para criar e aplicar migrations, não utilizada em tempo de execução
/// </summary>
public class LoadContextDesignFactory : IDesignTimeDbContextFactory<LoadContext>
{
    public LoadContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LoadContext>();
        optionsBuilder
            .UseNpgsql(Consts.ConnectionString)
            .UseSnakeCaseNamingConvention();
        return new LoadContext(optionsBuilder.Options);
    }
}