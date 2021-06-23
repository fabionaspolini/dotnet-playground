using System;
using static System.Console;

WriteLine(".:: Maestria Type Providers ::.");

// Carregando dados de excel com tipagem forte gerada automaticamente
var dadosTemplate = PessoaFactory.Load("./Dados/Pessoa.xlsx");

WriteLine($"| {"Id",4} | {"Nome",-20} | {"Nascimento",-10} | {"Cidade",-20} | {"Renda",14} |");
WriteLine(new string('=', 84));
foreach (var item in dadosTemplate)
    WriteLine($"| {item.Id,4} | {item.Nome,-20} | {item.DataNascimento?.ToString("dd/MM/yyyy"),-10} | {item.Cidade,-20} | {item.RendaMedia?.ToString("C2"),14} |");

// O arquivo template deve ser pequeno, o objetivo é apenas mapea-lo para gerar as classes. Quando menor, mais rápido será o build.
[ExcelProvider(TemplatePath = "./Templates/Pessoa.xlsx")]
public partial class Pessoa
{
}