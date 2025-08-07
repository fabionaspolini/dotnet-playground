namespace efcore_zextensions_playground.Entities;

public class Pessoa
{
    public Pessoa(int id, string nome, string documento, string cidade)
    {
        Id = id;
        Nome = nome;
        Documento = documento;
        Cidade = cidade;

    }
    public int Id { get; set; }
    public string Nome { get; set; }

    /// <summary>
    /// CPF/CNPJ
    /// </summary>
    public string Documento { get; set; }
    public string Cidade { get; set; }
}