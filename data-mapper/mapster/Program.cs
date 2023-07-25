using Mapster;
using MapsterPlayground;

// Obrigar que todos os mapeamentos estejam registrados
TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = true;

// Obrigar que todos os mapeamentos esteja explicitos e antecipar a descoberta de incompatbilidades causadas por mudanças no código.
// Se criar uma propriedade nova no objeto destino sem equivalência na origem, ocorrerá erro em runtime no momento da execução do método "Adapt<>",
TypeAdapterConfig.GlobalSettings.RequireDestinationMemberSource = true;

TypeAdapterConfig<Person, PersonModel>.NewConfig()
    .Ignore(d => d.LastName!);
TypeAdapterConfig<Person, PersonModelReadOnly>.NewConfig();

TypeAdapterConfig<Person, PersonModelDif>
    .NewConfig()
    .Map(d => d.Codigo, s => s.Id)
    .Map(d => d.Nome, s => s.Name);

TypeAdapterConfig<Person, PersonModelDifReadonly>
    .NewConfig()
    .Map(d => d.Codigo, s => s.Id)
    .Map(d => d.Nome, s => s.Name);

// Validar configuração para evitar erros de mapeamento em runtime e antecipar a descoberta de incompatbilidades causadas por mudanças no código.
// Você será alertado no startup da aplicação quando:
// 1: Se criar uma propriedade nova no objeto destino sem equivalência na origem
TypeAdapterConfig.GlobalSettings.Compile();


var person = new Person(
    id: Guid.Parse("85246202-282b-48cc-bea8-bab1447f07df"),
    name: "Fulano",
    birthDate: DateTime.Now);

var personModel = person.Adapt<PersonModel>();
var personModelReadOnly = person.Adapt<PersonModelReadOnly>();
var personModelDif = person.Adapt<PersonModelDif>();
var personModelDifReadonly = person.Adapt<PersonModelDifReadonly>();

Console.WriteLine($"PersonModel             => {personModel.Id}: {personModel.Name}");
Console.WriteLine($"PersonModelReadOnly     => {personModel.Id}: {personModel.Name}");
Console.WriteLine($"PersonModelDif          => {personModelDif.Codigo}: {personModelDif.Nome}");
Console.WriteLine($"PersonModelDifReadOnly  => {personModelDifReadonly.Codigo}: {personModelDifReadonly.Nome}");

// Mapear por cima de uma instância
var person2 = new Person(
    id: Guid.Parse("85246202-282b-48cc-bea8-bab1447f07df"),
    name: "Fulano de tal",
    birthDate: DateTime.Now);

// Passei por problema em outr projeto quando havia uma classe destino com propriedade readonly impactar em propriedades writeables, não causava exception mas não fazia certo...causando falsa impressão de sucesso.
// Não consegui montar o cenário aqui. No outro projeto também estava utilizando a feature em conjunto com injenção de dependência
person2.Adapt(personModel);
person2.Adapt(personModelDif);

Console.WriteLine($"[person2] PersonModel    => {personModel.Id}: {personModel.Name}");
Console.WriteLine($"[person2] PersonModelDif => {personModelDif.Codigo}: {personModelDif.Nome}");

// Feature de geração de código, não obirgatório
public class CodeGenerationConfig : ICodeGenerationRegister
{
    public void Register(Mapster.CodeGenerationConfig config)
    {
        // No arquivo .csproj existem tasks para geração de código automática, mas deixei desativadas por padrão.
        // Executar manual "dotnet msbuild -t:Mapster" para criar as classes DTO e Mappers com o sufixo de "Test" configuradas abaixo.
        config.AdaptTo("[name]ModelTest")
            .ForType<Person>();
        config.GenerateMapper("[name]MapperTest")
            .ForType<Person>();
    }
}