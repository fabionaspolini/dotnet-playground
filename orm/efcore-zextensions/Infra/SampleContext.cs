using efcore_zextensions_playground.Entities;
using efcore_zextensions_playground.Infra.Mappings;
using Microsoft.EntityFrameworkCore;

namespace efcore_zextensions_playground.Infra;

public class SampleContext : DbContext
{
    // protected SampleContext()
    // {
    // }

    public SampleContext(DbContextOptions<SampleContext> options) : base(options)
    {
    }

    public DbSet<Pessoa> Pessoas { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new PessoaMap());
    }
}