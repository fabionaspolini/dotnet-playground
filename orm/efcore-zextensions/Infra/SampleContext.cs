using EFCore_ZExtensionsPlayground.Entities;
using EFCore_ZExtensionsPlayground.Infra.Mappings;
using Microsoft.EntityFrameworkCore;

namespace EFCore_ZExtensionsPlayground.Infra;

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