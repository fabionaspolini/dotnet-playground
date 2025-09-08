using database_load_playground.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace database_load_playground.Db.Mappings;

public class MovimentacaoMap : IEntityTypeConfiguration<Movimentacao>
{
    public void Configure(EntityTypeBuilder<Movimentacao> builder)
    {
        builder.ToTable("movimentacao");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.ClienteId);
    }
}