using database_load_playground.Db.Mappings;
using database_load_playground.Entities;
using Microsoft.EntityFrameworkCore;

namespace database_load_playground.Db;

public class LoadContext : DbContext
{
    protected LoadContext()
    {
    }
        
    public LoadContext(DbContextOptions<LoadContext> options) : base(options)
    {
    }

    public DbSet<Transacao> Transacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TransacaoConfig());
    }
}