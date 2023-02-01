using Mapster;
using Mapster_Sample;

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

Console.WriteLine($"PersonModel             => {personModel.Id}: {personModel.Name}");
Console.WriteLine($"PersonModelReadOnly     => {personModel.Id}: {personModel.Name}");
Console.WriteLine($"PersonModelDif          => {personModelDif.Codigo}: {personModelDif.Nome}");
Console.WriteLine($"PersonModelDifReadOnly  => {personModelDifReadonly.Codigo}: {personModelDifReadonly.Nome}");

// public class MyRegister : ICodeGenerationRegister
// {
//     public void Register(CodeGenerationConfig config)
//     {
//         // config.AdaptTo("[name]Dto")
//         //     .ForAllTypesInNamespace(Assembly.GetExecutingAssembly(), "Mapster_Sample");

//         config.GenerateMapper("[name]Mapper")
//                 .ForType<Course>()
//                 .ForType<Student>();
//     }
// }