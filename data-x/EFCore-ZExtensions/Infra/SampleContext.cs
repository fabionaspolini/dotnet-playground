using EFCore_ZExtensions_Sample.Entities;
using EFCore_ZExtensions_Sample.Infra.Mappings;
using Microsoft.EntityFrameworkCore;

namespace EFCore_ZExtensions_Sample.Infra;

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