using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Maestria.Extensions;
using Maestria.Extensions.DataTypes;
using static System.Console;

WriteLine(".:: Maestria Extensions ::.");

// Variável de apoio
IEnumerable<int> list = new List<int>() { 10, 20, 30 };
IEnumerable<int> nullList = null;
IEnumerable<int> emptyList = new List<int>();
var stringList = new List<string> { "Maestria", "Extensions", "Methods" };
var dic = new Dictionary<string, object>
{
    {"nome", "Fulano"},
    {"idade", 25}
};
var exception = new Exception("Nivel 3",
    new Exception("Nivel 2",
        new Exception("Nivel 1")));

var cor = Color.Red;
var numero = 10;
var numeroDecimal = 10.12345;
var texto = "maestria";
string nullString = null;
var textoToFormat = "Nome: {0}";
var frase = "maestria-extensions-functions";
var path = "/path/to/save";

// Extensões
WriteLine("=====> Base64 Extensions <=====");
WriteLine($"Encode: {"Maestria Demo".ToBase64()}");
WriteLine($"Decode: {"TWFlc3RyaWEgRGVtbw==".FromBase64()}");
WriteLine();

WriteLine("=====> Collection Extensions <=====");
list.Iterate(item => WriteLine($"Iterate(item): {item}"));
list.Iterate((item, index) => WriteLine($"Iterate(item, index): {item}, {index}"));
await list.Iterate(async item => WriteLine($"Async Iterate(item): {item}"));
await list.Iterate(async (item, index) => WriteLine($"Async Iterate(item, index): {item}, {index}"));
WriteLine();

WriteLine("foreach ... IEnumerable.WithIndex()");
foreach (var (value, index) in list.WithIndex())
    WriteLine($"    value: {value}, index: {index}");
WriteLine();

WriteLine("IEnumerable.IsNullOrEmpty()");
WriteLine($"    list.IsNullOrEmpty()     : {list.IsNullOrEmpty()}");
WriteLine($"    nullList.IsNullOrEmpty() : {nullList.IsNullOrEmpty()}");
WriteLine($"    emptyList.IsNullOrEmpty(): {emptyList.IsNullOrEmpty()}");
WriteLine();

WriteLine("IEnumerable.HasItems()");
WriteLine($"    list.HasItems()     : {list.HasItems()}");
WriteLine($"    nullList.HasItems() : {nullList.HasItems()}");
WriteLine($"    emptyList.HasItems(): {emptyList.HasItems()}");
WriteLine();

WriteLine("IDictionary.TryGetValue(chave)");
WriteLine($"    dic.TryGetValue(\"nome\")       : {dic.TryGetValue("nome", string.Empty)}");
WriteLine($"    dic.TryGetValue(\"invalid_key\"): {dic.TryGetValue("invalid_key", string.Empty)}");
WriteLine();

WriteLine("=====> Enum Extensions <=====");
WriteLine("Enum.GetDisplayName()");
WriteLine($"    Color.Red.GetDisplayName()  : {Color.Red.GetDisplayName()}");
WriteLine($"    Color.Blue.GetDisplayName() : {Color.Blue.GetDisplayName()}");
WriteLine($"    Color.Green.GetDisplayName(): {Color.Green.GetDisplayName()}");
WriteLine();

WriteLine("Enum.GetDescription()");
WriteLine($"    Color.Red.GetDescription()  : {Color.Red.GetDescription()}");
WriteLine($"    Color.Blue.GetDescription() : {Color.Blue.GetDescription()}");
WriteLine($"    Color.Green.GetDescription(): {Color.Green.GetDescription()}");
WriteLine();

WriteLine("EnumExtensions.GetValues()");
WriteLine($"    EnumExtensions.GetValues<Color>()      : {EnumExtensions.GetValues<Color>().Select(x => x.ToString()).Join(", ")}");
WriteLine($"    EnumExtensions.GetValues(typeof(Color)): {EnumExtensions.GetValues(typeof(Color)).Select(x => x.ToString()).Join(", ")}");
WriteLine();

WriteLine("=====> Exception Extensions <=====");
WriteLine($"exception.GetMostInner()  : {exception.GetMostInner().Message}");
WriteLine($"exception.GetAllMessages(): {exception.GetAllMessages()}");
WriteLine();

WriteLine("=====> Hash Extensions <=====");
WriteLine($"\"maestria\".GetHashMd5()   : {"maestria".GetHashMd5()}");
WriteLine($"\"maestria\".GetHashSha1()  : {"maestria".GetHashSha1()}");
WriteLine($"\"maestria\".GetHashSha256(): {"maestria".GetHashSha256()}");
WriteLine($"\"maestria\".GetHashSha384(): {"maestria".GetHashSha384()}");
WriteLine($"\"maestria\".GetHashSha512(): {"maestria".GetHashSha512()}");
WriteLine();

WriteLine("=====> Syntax Extensions <=====");
// Métodos para validação == null ou != null
WriteLine("null check");
WriteLine($"    list.IsNull()       : {list.IsNull()}");
WriteLine($"    list.HasValue()     : {list.HasValue()}");
WriteLine($"    list.IsNotNull()    : {list.IsNotNull()}"); // IsNotNull e HasValue possuem o mesmo resultado
WriteLine($"    nullList.IsNull()   : {nullList.IsNull()}");
WriteLine($"    nullList.HasValue() : {nullList.HasValue()}");
WriteLine($"    nullList.IsNotNull(): {nullList.IsNotNull()}"); // IsNotNull e HasValue possuem o mesmo resultado
WriteLine();

// Método "In" para inverter e facilitar a leitura de "new[] { Color.Green, Color.Blue }.Contains(cor)".
// Funciona para qualquer tipo de dado
WriteLine("object.In(...)");
WriteLine($"    cor.In(Color.Green, Color.Blue): {cor.In(Color.Green, Color.Blue)}");
WriteLine($"    cor.In(Color.Red, Color.Blue)  : {cor.In(Color.Red, Color.Blue)}");
WriteLine($"    numero.In(20, 30)              : {numero.In(20, 30)}");
WriteLine();

// Método "Between" para facilitar a escrita de "variavel >= inicio && variavel <= fim".
// Funciona para tipos de dados comparáveis (Extensões de IComparable)
WriteLine("IComparable");
WriteLine($"    numero.Between(20, 30): {numero.Between(20, 30)}");
WriteLine($"    numero.Between(5, 10) : {numero.Between(5, 10)}");
WriteLine($"    numero.Between(5, 15) : {numero.Between(5, 15)}");
WriteLine($"    numero.LimitMaxAt(15): {numero.LimitMaxAt(15)}"); // Limitar valor máximo da variável com o valor do argumento indicado
WriteLine($"    numero.LimitMaxAt(5) : {numero.LimitMaxAt(5)}");
WriteLine($"    numero.LimitMinAt(15): {numero.LimitMinAt(15)}"); // Limitar valor mínimo da variável com o valor do argumento indicado
WriteLine($"    numero.LimitMinAt(5) : {numero.LimitMinAt(5)}");
WriteLine();

// Outras
var resultado = numero
    .LimitMaxAt(9)
    .OutVar(out var numeroLimitado)
    .DetachedInvoke(value => WriteLine($"    {value}"))
    .IfLessOrEqual(3).Then(-1);
WriteLine($"pipeline.OutVar(out var numeroLimitado).pipeline()"); // Expressão "OutVar" não modifica nada, simplesmente atributi o valor atual para uma variável fora do escopo do pipeline
WriteLine($"pipeline.DetachedInvoke(value => ...).pipeline()"); // Expressão "DetachedInvoke" faz uma chamada avulsa no pipeline e continuam a execução com o mesmo valor de entrada
WriteLine();

WriteLine("=====> If <=====");
WriteLine("IComparable");
WriteLine($"    numero.If(10).Then(20)              : {numero.If(10).Then(20)}");
WriteLine($"    numero.If(5).Then(20)               : {numero.If(5).Then(20)}");
WriteLine($"    numero.IfNot(10).Then(20)           : {numero.IfNot(10).Then(20)}");
WriteLine($"    numero.IfNot(5).Then(20)            : {numero.IfNot(5).Then(20)}");
WriteLine($"    numero.IfGreater(5).Then(20)        : {numero.IfGreater(5).Then(20)}");
WriteLine($"    numero.IfGreaterOrEqual(5).Then(20) : {numero.IfGreaterOrEqual(5).Then(20)}");
WriteLine($"    numero.IfLess(5).Then(20)           : {numero.IfLess(5).Then(20)}");
WriteLine($"    numero.IfLessOrEqual(5).Then(20)    : {numero.IfLessOrEqual(5).Then(20)}");
WriteLine();

WriteLine("=====> IfNull <=====");
WriteLine("object e struct");
WriteLine($"    nullList.IfNull(new List<int>()): {nullList.IfNull(new List<int>())}"); // Se a variavel for null, retorna o defaul indicado no argumento, caso contrário retorna a própria variável
WriteLine("string");
WriteLine($"    nullString.IfNullOrEmpty(\"vazio\")     : {nullString.IfNullOrEmpty("vazio")}");
WriteLine($"    nullString.IfNullOrWhiteSpace(\"vazio\"): {nullString.IfNullOrWhiteSpace("vazio")}");
WriteLine();

WriteLine("=====> NullIf <=====");
WriteLine("object e struct");
WriteLine($"    numero.NullIf(1)           : {numero.NullIf(1)}");
WriteLine($"    numero.NullIf(10)          : {numero.NullIf(10)}");
WriteLine($"    numero.NullIfIn(10, 20, 30): {numero.NullIfIn(10, 20, 30)}");
WriteLine($"    numero.NullIfBetween(5, 15): {numero.NullIfBetween(5, 15)}");
WriteLine("string");
WriteLine($"    texto.NullIfEmpty()     : {texto.NullIfEmpty()}");
WriteLine($"    texto.NullIfWhiteSpace(): {texto.NullIfWhiteSpace()}");
WriteLine();

WriteLine("=====> Round <=====");
WriteLine("decimal, double e float");
WriteLine($"    numeroDecimal         : {numeroDecimal}");
WriteLine($"    numeroDecimal.Round() : {numeroDecimal.Round()}"); // Round irá arredondar de acordo com a matemática, sendo possível indicar o algoritmo por parâmetro.
WriteLine($"    numeroDecimal.Round() : {numeroDecimal.RoundUp()}"); // RoundUp irá sempre arredondar para cima
WriteLine($"    numeroDecimal.Round(2): {numeroDecimal.Round(2)}");
WriteLine($"    numeroDecimal.Round(2): {numeroDecimal.RoundUp(2)}");
WriteLine();

WriteLine("=====> Truncate <=====");
WriteLine("decimal, double e float");
WriteLine($"    numeroDecimal            : {numeroDecimal}");
WriteLine($"    numeroDecimal.Truncate() : {numeroDecimal.Truncate()}"); // Truncar número / Arredondar para baixo
WriteLine($"    numeroDecimal.Truncate(2): {numeroDecimal.Truncate(2)}");
WriteLine();

WriteLine("=====> String <=====");
WriteLine("Útil");
WriteLine($"    texto.EqualsIgnoreCase(\"Maestria\")            : {texto.EqualsIgnoreCase("Maestria")}");
WriteLine($"    stringList.Join(\" \")                          : {stringList.Join(" ")}");
WriteLine($"    textoToFormat.Format(\"Maestria\")              : {textoToFormat.Format("Maestria")}");
WriteLine($"    \"123abcd456\".OnlyNumbers()                    : {"123abcd456".OnlyNumbers()}");
WriteLine($"    \"sem acentos: áéíóúàèìòùâêîôû\".RemoveAccents(): {"sem acentos: áéíóúàèìòùâêîôû".RemoveAccents()}");
WriteLine($"    texto.LimitLen(3)                             : {texto.LimitLen(3)}");
WriteLine($"    texto.LimitLenReverse(3)                      : {texto.LimitLenReverse(3)}");
WriteLine($"    texto.SaveAs(\"C:/Temp/teste.txt\")             : Salvar arquivo");
WriteLine($"    text.EscapeXml()                              : {texto.EscapeXml()}");
WriteLine("Trim");
WriteLine($"    text.TrimStart()                              : {texto.TrimStart()}");
WriteLine($"    text.TrimStart(\"ma\")                          : {texto.TrimStart("ma")}");
WriteLine($"    text.TrimEnd()                                : {texto.TrimEnd()}");
WriteLine($"    text.TrimEnd(\"ia\")                            : {texto.TrimEnd("ia")}");
WriteLine("Substring");
WriteLine($"     frase.SubstringBeforeFirstOccurrence(\"-\")    : {frase.SubstringBeforeFirstOccurrence("-")}");
WriteLine($"     frase.SubstringBeforeLastOccurrence(\"-\")     : {frase.SubstringBeforeLastOccurrence("-")}");
WriteLine($"     frase.SubstringAfterFirstOccurrence(\"-\")     : {frase.SubstringAfterFirstOccurrence("-")}");
WriteLine($"     frase.SubstringAfterLastOccurrence(\"-\")      : {frase.SubstringAfterLastOccurrence("-")}");
WriteLine($"     frase.SubstringAtOccurrenceIndex(\"-\", 1)     : {frase.SubstringAtOccurrenceIndex("-", 1)}");
WriteLine($"     frase.SubstringSafe(10, 1000)                : {frase.SubstringSafe(10, 1000)}"); // Não da exception e assume o tamanho da string como máximo possível
WriteLine("If");
WriteLine($"    texto.IsNullOrEmpty()                         : {texto.IsNullOrEmpty()}");
WriteLine($"    texto.IsNullOrWhiteSpace()                    : {texto.IsNullOrWhiteSpace()}");
WriteLine($"    texto.HasValue()                              : {texto.HasValue()}"); // Não é nulo nem espaço vazio
WriteLine($"    texto.EmptyIf(\"maestria\")                     : {texto.EmptyIf("maestria")}");
WriteLine($"    texto.EmptyIfNull()                           : {texto.EmptyIfNull()}");
WriteLine($"    texto.EmptyIfNullOrWhiteSpace()               : {texto.EmptyIfNullOrWhiteSpace()}");
WriteLine($"    path.AddToBeginningIfNotStartsWith(\"/\")       : {path.AddToBeginningIfNotStartsWith("/")}");
WriteLine($"    path.AddToEndIfNotEndsWith(\"/\")               : {path.AddToEndIfNotEndsWith("/")}");
WriteLine($"    path.AddToBeginningIfHasValue(\"Salvar em => \"): {path.AddToBeginningIfHasValue("Salvar em => ")}");
WriteLine($"    path.AddToEndIfHasValue(\" <= salvo\")          : {path.AddToEndIfHasValue(" <= salvo")}");
WriteLine();

WriteLine("=====> SimpleResult <=====");
// Os Tipos SimpleResult e SimpleResult<TValue> facilitam para aplicar o conceito de não executar exceptions no domínio da aplicação com seus operadores implicitos.
var testeConversao1 = ConverterParaInt("15");
var testeConversao2 = ConverterParaInt("");
var testeConversao3 = ConverterParaInt("aaa");

if (testeConversao1) // Implicit cast para boolean ok.Success
{
    int valor = testeConversao1; // Implicit cast do valor ok.Value;
    WriteLine($"Valor convertido: {valor}");
}

if (!testeConversao2)
    WriteLine($"testeConversao2.Message: {testeConversao2.Message}");

if (!testeConversao3)
{
    WriteLine($"testeConversao3.Message: {testeConversao3.Message}");
    WriteLine($"testeConversao3.Exception: {testeConversao3.Exception.GetAllMessages()}");
}
WriteLine();

SimpleResult<int> ConverterParaInt(string texto)
{
    if (texto.IsNullOrWhiteSpace())
        return "Texto não informado para conversão!"; // Implicit cast para falha e mensagem
    try
    {
        return Convert.ToInt32(texto); // Implicit cast para sucesso e resultado
    }
    catch (Exception e)
    {
        return e; // Implicit cast para falha retornando os dados da exception
    }
}

WriteLine("=====> Try <=====");
// O tipo Try<TSuccess, TFailure> serve para enriquecimento do dominio, permitindo expressar uma função com dois tipos de retornos diferentes para sucesso e falha.
var result = TentarIntegrarPessoa("maestria");
if (result) // Implicit cast para result.Successfully
{
    int id = result; // Implicit cast para result.Success
    WriteLine($"Sucesso ao integrar pessoa, id: {id}");
}
else
{
    string message = result; // Implicit cast para result.Failure
    WriteLine($"Falha ao integrar pessoa, mensagem: {message}");
}

Try<int, string> TentarIntegrarPessoa(string nome)
{
    try
    {
        // Rotina para integra em sistema externo, se der certo retorna o Id
        if (nome == "")
            throw new Exception("Simulação de erro");
        return 10;
    }
    catch (Exception e)
    {
        return e.Message;
    }
}

// Types
public enum Color
{
    [Display(Name = "Vermelho", Description = "Vermelho desc")]
    Red,

    [Description("Azul desc")]
    Blue,

    [Description("Verde desc from DescriptionAttribute")]
    [Display(Name = "Verde", Description = "Não usado nesta configuração")]
    Green
}
