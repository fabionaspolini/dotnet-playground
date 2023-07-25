using System;

namespace LanguageFeaturesPlayground.Immutable
{
    public class Tests
    {
        public static void Execute()
        {
            // ----- Classe com propriedades "initilizer" -----
            var pessoa = new PessoaClass
            {
                Id = 1,
                Nome = "AAA"
            };
            pessoa.Imprimir();
            // pessoa.Nome = "aaa"; // Error

            // ----- Classe do tipo "record". Esta possui o método "with" para copiar o objeto -----
            var pessoaRecord = new PessoaRecord
            {
                Id = 1,
                Nome = "Fábio"
            };
            var pessoaRecord2 = pessoaRecord with { Sobrenome = "Naspolini" };
            pessoaRecord.Imprimir();
            pessoaRecord2.Imprimir();

            var pessoaRecord3 = new PessoaRecord
            {
                Id = 1,
                Nome = "Fábio",
                Sobrenome = "Naspolini"
            };
            pessoaRecord3.Imprimir();
            Console.WriteLine($"pessoaRecord == pessoaRecord2 {pessoaRecord == pessoaRecord2}");
            Console.WriteLine($"pessoaRecord2 == pessoaRecord3 {pessoaRecord2 == pessoaRecord3}");

            var pessoaRecord4 = new PessoaRecord(1, "Fábio", "Naspolini");
            pessoaRecord4.Imprimir();
            Console.WriteLine($"pessoaRecord2 == pessoaRecord4 {pessoaRecord2 == pessoaRecord4}");

            // ----- Classe do tipo "record" em uma linha -----
            var teste = new PessoaRecord2(1, "Fábio", "Naspolini");
            var teste2 = teste with { Sobrenome = "AAAAAAAA" };
            teste.Imprimir();
            teste2.Imprimir();
        }
    }
}