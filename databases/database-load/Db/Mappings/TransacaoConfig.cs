using database_load_playground.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace database_load_playground.Db.Mappings;

public class TransacaoConfig : IEntityTypeConfiguration<Transacao>
{
    public void Configure(EntityTypeBuilder<Transacao> builder)
    {
        builder.ToTable("transacao");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.ClienteId);
        
        builder.Property(x => x.Valor).HasPrecision(12, 2);
        builder.Property(x => x.Descricao).HasMaxLength(40);
    }
}