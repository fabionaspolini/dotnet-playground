using AutoMapper;
using AutoMapper_Sample;

var person = new Person(
    id: Guid.Parse("85246202-282b-48cc-bea8-bab1447f07df"),
    name: "Fulano",
    birthDate: DateTime.Now);

var config = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Person, PersonModel>()
        .ForMember(d => d.LastName, opts => opts.Ignore());

    cfg.CreateMap<Person, PersonModelReadOnly>(); // Construtor readonly com nome igual OK

    cfg.CreateMap<Person, PersonModelDif>()
        .ForMember(d => d.Codigo, otps => otps.MapFrom(s => s.Id))
        .ForMember(d => d.Nome, otps => otps.MapFrom(s => s.Name));

    cfg.CreateMap<Person, PersonModelDifReadonly>() // Construtor readonly com nomes diferentes, precisa de customização
        .ConstructUsing(s => new(s.Id, s.Name))
        .ForAllMembers(otps => otps.Ignore());
});

config.CompileMappings(); // Compilar mapeamentos no startup da aplicação. Sem isso ocorrerá on-demand.

try
{
    // Validar configuração para evitar erros de mapeamento em runtime e antecipar a descoberta de incompatbilidades causadas por mudanças no código.
    // Você será alertado no startup da aplicação quando:
    // 1: Se criar uma propriedade nova no objeto destino sem equivalência na origem
    // 2: Construtor do objeto destino incompativel
    config.AssertConfigurationIsValid();
}
catch (Exception e)
{
    // Esse try catch é apenas para facilitar debugger no VS Code que estava conseguindo pegar o stacktrace completo antes do shutdown da aplicação
    Console.WriteLine(e);
    throw;
}

var mapper = new Mapper(config);

var personModel = mapper.Map<PersonModel>(person);
var personModelReadOnly = mapper.Map<PersonModelReadOnly>(person);
var personModelDif = mapper.Map<PersonModelDif>(person);
var personModelDifReadonly = mapper.Map<PersonModelDifReadonly>(person);

Console.WriteLine($"PersonModel             => {personModel.Id}: {personModel.Name}");
Console.WriteLine($"PersonModelReadOnly     => {personModel.Id}: {personModel.Name}");
Console.WriteLine($"PersonModelDif          => {personModelDif.Codigo}: {personModelDif.Nome}");
Console.WriteLine($"PersonModelDifReadOnly  => {personModelDifReadonly.Codigo}: {personModelDifReadonly.Nome}");