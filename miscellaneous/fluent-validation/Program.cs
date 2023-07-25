using System;
using FluentValidation;
using static System.Console;

namespace FluentValidationPlayground
{
    class Program
    {
        public static Cidade[] Cidades = new Cidade[] { new(1, "Criciúma"), new(2, "Curitiba") };
        PessoaValidator pessoaValidator = new PessoaValidator();

        static void Main(string[] args)
        {
            WriteLine(".:: FluentValidation Samples ::.");

            ValidatorOptions.Global.CascadeMode = CascadeMode.StopOnFirstFailure;

            var test = new Program();
            test.Validar("Todos os dados em vazios", new Pessoa());

            test.Validar("Pessoa com id inválido de cidade", new Pessoa
            {
                Id = Guid.NewGuid(),
                Tipo = TipoPessoa.Fisica,
                Nome = "Fábio",
                Email = "fabio@mail.com",
                Documento = "07072034900",
                CidadeId = 10,
                Fisica = new()
                {
                    NomeMae = "Fulana de tal"
                }
            });

            // Física
            test.Validar("Pessoa física com tipo x número documento incoerente e nome sem tamanho mínimo", new Pessoa
            {
                Id = Guid.NewGuid(),
                Tipo = TipoPessoa.Juridica,
                Nome = "F",
                Documento = "07072034900",
                CidadeId = 1
            });

            test.Validar("Pessoa física sem nome de mãe", new Pessoa
            {
                Id = Guid.NewGuid(),
                Tipo = TipoPessoa.Fisica,
                Nome = "Fábio",
                Documento = "07072034900",
                CidadeId = 1,
                Fisica = new()
                {
                }
            });

            test.Validar("Pessoa física com email inválido", new Pessoa
            {
                Id = Guid.NewGuid(),
                Tipo = TipoPessoa.Fisica,
                Nome = "Fábio",
                Email = "fabio",
                Documento = "07072034900",
                CidadeId = 1,
                Fisica = new()
                {
                    NomeMae = "Fulana de tal"
                }
            });

            test.Validar("Pessoa física com todos os dados preenchidos", new Pessoa
            {
                Id = Guid.NewGuid(),
                Tipo = TipoPessoa.Fisica,
                Nome = "Fábio",
                Email = "fabio@mail.com",
                Documento = "07072034900",
                CidadeId = 1,
                Fisica = new()
                {
                    NomeMae = "Fulana de tal"
                }
            });

            // Jurídica
            test.Validar("Pessoa jurídica sem razão social", new Pessoa
            {
                Id = Guid.NewGuid(),
                Tipo = TipoPessoa.Juridica,
                Nome = "Empresa X",
                Documento = "12345678901234",
                CidadeId = 1
            });

            test.Validar("Pessoa jurídica com razão social vazia", new Pessoa
            {
                Id = Guid.NewGuid(),
                Tipo = TipoPessoa.Juridica,
                Nome = "Empresa X",
                Documento = "12345678901234",
                CidadeId = 1,
                Juridica = new()
                {
                }
            });

            test.Validar("Pessoa jurídica com todos os dados preenchidos", new Pessoa
            {
                Id = Guid.NewGuid(),
                Tipo = TipoPessoa.Juridica,
                Nome = "Empresa X",
                Documento = "12345678901234",
                CidadeId = 1,
                Juridica = new()
                {
                    RazaoSocial = "Empresa X Ltda"
                }
            });
        }

        static int _count = 1;
        void Validar(string message, Pessoa pessoa)
        {
            var result = pessoaValidator.Validate(pessoa);
            if (result.IsValid)
                WriteLine($"{_count}: {message} => OK");
            else
            {
                WriteLine($"{_count}: {message} => Problemas encontrados:");
                WriteLine(result.ToString());
                /*ou ... foreach (var item in result.Errors)
                    WriteLine(item);*/
            }
            WriteLine();
            _count++;
        }
    }
}
