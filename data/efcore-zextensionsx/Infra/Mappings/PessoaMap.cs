using EFCore_ZExtensions_Sample.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore_ZExtensions_Sample.Infra.Mappings;

public class PessoaMap : IEntityTypeConfiguration<Pessoa>
{
    public void Configure(EntityTypeBuilder<Pessoa> builder)
    {
        builder.ToTable("cliente");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Nome).HasMaxLength(60);
        builder.Property(x => x.Documento).HasMaxLength(14);
        builder.Property(x => x.Cidade).HasMaxLength(60);
    }
}