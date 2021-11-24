namespace LanguageFeatures_Sample.Immutable
{
    /// <summary>
    /// Classe readonly que só pode ter as propriedades atribuida na criação, mas deixando todas as propriedades explicitas no código.
    /// </summary>
    public class PessoaClass
    {
        public int Id { get; init; }
        public string Nome { get; init; }
        public string Sobrenome { get; init; }
        public void Imprimir() => System.Console.WriteLine($"PessoaClass => {Id} - {Nome} {Sobrenome}");
    }

    public record PessoaRecord
    {
        public PessoaRecord()
        {
        }

        public PessoaRecord(int id, string nome, string sobrenome)
        {
            Id = id;
            Nome = nome;
            Sobrenome = sobrenome;
        }

        public int Id { get; init; }
        public string Nome { get; init; }
        public string Sobrenome { get; init; }

        public void Imprimir() => System.Console.WriteLine($"PessoaRecord => {Id} - {Nome} {Sobrenome}");
    }

    public record PessoaRecord2(
        int Id,
        string Nome,
        string Sobrenome)
    {
        public void Imprimir() => System.Console.WriteLine($"PessoaRecord2 => {Id} - {Nome} {Sobrenome}");
    }
}