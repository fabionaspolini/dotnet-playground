using Mapster;
using Mapster_Sample;
using MapsterMapper;

// TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = true;
TypeAdapterConfig<Person, PersonModelDif>
    .NewConfig()
    .Map(t => t.Codigo, s => s.Id)
    .Map(t => t.Nome, s => s.Name);

TypeAdapterConfig<Person, PersonModelDifReadonly>
    .NewConfig()
    .Map(t => t.Codigo, s => s.Id)
    .Map(t => t.Nome, s => s.Name);


var person = new Person(
    id: Guid.Parse("85246202-282b-48cc-bea8-bab1447f07df"),
    name: "Fulano");

var personModel = person.Adapt<PersonModel>();
var personModelReadOnly = person.Adapt<PersonModelReadOnly>();
var personModelDif = person.Adapt<PersonModelDif>();
var personModelDifReadonly = person.Adapt<PersonModelDifReadonly>();

// var test = person.Adapt<PersonModelTest>();

Console.WriteLine($"PersonModel             => {personModel.Id}: {personModel.Name}");
Console.WriteLine($"PersonModelReadOnly     => {personModel.Id}: {personModel.Name}");
Console.WriteLine($"PersonModelDif          => {personModelDif.Codigo}: {personModelDif.Nome}");
Console.WriteLine($"PersonModelDifReadOnly  => {personModelDifReadonly.Codigo}: {personModelDifReadonly.Nome}");

public class CodeGenerationConfig : ICodeGenerationRegister
{
    public void Register(Mapster.CodeGenerationConfig config)
    {
        // Build tasks generate mapping and dto class at model "Models"
        config.AdaptTo("[name]ModelTest")
            .ForType<Person>();
        config.GenerateMapper("[name]MapperTest")
            .ForType<Person>();
    }
}