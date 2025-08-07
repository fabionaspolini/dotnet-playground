using System;

namespace fluent_validation_playground;

enum TipoPessoa
{
    Fisica,
    Juridica
}

class Pessoa
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public TipoPessoa Tipo { get; set; }
    public string Documento { get; set; }
    public int CidadeId { get; set; }
    public PessoaFisica Fisica { get; set; }
    public PessoaJuridica Juridica { get; set; }
}

class PessoaFisica
{
    public string NomeMae { get; set; }
    public string NomePai { get; set; }
}

class PessoaJuridica
{
    public string RazaoSocial { get; set; }
}

class Cidade
{
    public Cidade()
    {
    }

    public Cidade(int id, string nome)
    {
        Id = id;
        Nome = nome;
    }

    public int Id { get; set; }
    public string Nome { get; set; }
}