using FluentNHibernate.Mapping;

namespace NHibernate_Sample;

public class PessoaMap : ClassMap<Pessoa>
{
    public PessoaMap()
    {
        Id(x => x.Id).GeneratedBy.Assigned();
        Map(x => x.Nome).Length(60).Not.Nullable();
        Map(x => x.Apelido).Length(60).Not.Nullable();
        References<Cidade>(x => x.Cidade).Column("cidade_id").Not.Nullable();
    }
}

public class CidadeMap : ClassMap<Cidade>
{
    public CidadeMap()
    {
        Id(x => x.Id).GeneratedBy.Assigned();
        Map(x => x.Nome).Length(60).Not.Nullable();
    }
}
