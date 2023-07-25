using Fluid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Console;

WriteLine(".:: Liquid Samples ::.");
var dadosRelatorio = new Relatorio
{
    Titulo = "Detalhes do cliente",
    Nome = "Fulano de tal",
    DataNascimento = new DateTime(1989, 01, 01),
    Contatos = new() { "Pedro", "Paulo", "José" }
};


var templateStr = await File.ReadAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../relatorio.liquid"), Encoding.UTF8);
var parser = new FluidParser();
if (parser.TryParse(templateStr, out var template, out var error))
{
    var context = new TemplateContext(dadosRelatorio);
    var result = template.Render(context);
    WriteLine(result);
}
else
    WriteLine($"Error: {error}");

class Relatorio
{
    public string Titulo { get; set; }
    public string Nome { get; set; }
    public DateTime DataNascimento { get; set; }
    public List<string> Contatos { get; set; }
}