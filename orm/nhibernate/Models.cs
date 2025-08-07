namespace nhibernate_playground;

public class Pessoa
{
    // Obrigatório construtor vazio para funcionr com NHibernate
    protected Pessoa()
    {
    }

    public Pessoa(Guid id, string nome, string apelido, Cidade cidade)
    {
        Id = id;
        Nome = nome;
        Apelido = apelido;
        Cidade = cidade;
    }

    public virtual Guid Id { get; set; }
    public virtual string Nome { get; set; }
    public virtual string Apelido { get; set; }
    public virtual Cidade Cidade { get; set; }
}

public class Cidade
{
    protected Cidade()
    {
    }

    public Cidade(int id, string nome)
    {
        Id = id;
        Nome = nome;
    }

    public virtual int Id { get; set; }
    public virtual string Nome { get; set; }
}