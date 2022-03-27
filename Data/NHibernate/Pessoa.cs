namespace NHibernate_Sample;

public class Pessoa
{
    // Obrigatório construtor vazio para funcionr com NHibernate
    protected Pessoa()
    {
    }

    public Pessoa(Guid id, string nome, string apelido)
    {
        Id = id;
        Nome = nome;
        Apelido = apelido;
    }

    public virtual Guid Id { get; set; }
    public virtual string Nome { get; set; }
    public virtual string Apelido { get; set; }
}
