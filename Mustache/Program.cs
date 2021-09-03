using Stubble.Core.Builders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Console;

WriteLine(".:: Mustache Samples ::.");
var dadosRelatorio = new Relatorio
{
    Titulo = "Detalhes do cliente",
    Nome = "Fulano de tal",
    DataNascimento = new DateTime(1989, 01, 01),
    Contatos = new() { "Pedro", "Paulo", "José" }
};

var template = await File.ReadAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../relatorio.mustache"), Encoding.UTF8);
var render = new StubbleBuilder().Build();
var result = await render.RenderAsync(template, dadosRelatorio);
WriteLine(result);

//
var dadosRelatorioAsDictionary = new Dictionary<string, object>()
{
    { "Titulo", "Detalhes do cliente"},
    { "Nome", "Fulano de tal"},
    { "DataNascimento", new DateTime(1989, 01, 01)},
    { "Contatos", new List<string>() { "Pedro", "Paulo", "José" } }
};
var result2 = await render.RenderAsync(template, dadosRelatorioAsDictionary);
WriteLine(result);

class Relatorio
{
    public string Titulo { get; set; }
    public string Nome { get; set; }
    public DateTime DataNascimento { get; set; }
    public bool MaiorDeIdade => DateTime.Now - DataNascimento >= TimeSpan.FromDays(18 * 365);
    public List<string> Contatos { get; set; }
    public bool PossuiContatos => Contatos != null && Contatos.Count > 0;
}
