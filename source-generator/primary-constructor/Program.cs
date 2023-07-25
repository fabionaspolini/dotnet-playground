using System;
using static System.Console;

Console.WriteLine(".:: Primary Constructor ::.");

// Lib não suporta classes top level statements
namespace PrimaryConstructorPlayground
{
    class PessoaRepository { }

    class EmpresaRepository { }

    [PrimaryConstructor]
    partial class MeuServico
    {
        private readonly PessoaRepository _pessoaRepository;
        public EmpresaRepository EmpresaRepository { get; }

        [IgnorePrimaryConstructor]
        private readonly int _variavel_ignorada;

        [IncludePrimaryConstructor]
        public EmpresaRepository PropriedadeAdicionada { get; set; }
    }
}