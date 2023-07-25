using AutoMapper;
using AutoMapperPlayground;

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

// Validar configuração para evitar erros de mapeamento em runtime e antecipar a descoberta de incompatbilidades causadas por mudanças no código.
// Você será alertado no startup da aplicação quando:
// 1: Se criar uma propriedade nova no objeto destino sem equivalência na origem
// 2: Construtor do objeto destino incompativel (Parâmetro com nome diferente do campo origem não é suportado "facilmente")
config.AssertConfigurationIsValid();
config.CompileMappings(); // Compilar mapeamentos no startup da aplicação. Sem isso ocorrerá on-demand.

var mapper = new Mapper(config);

var person = new Person(
    id: Guid.Parse("85246202-282b-48cc-bea8-bab1447f07df"),
    name: "Fulano",
    birthDate: DateTime.Now);

var personModel = mapper.Map<PersonModel>(person);
var personModelReadOnly = mapper.Map<PersonModelReadOnly>(person);
var personModelDif = mapper.Map<PersonModelDif>(person);
var personModelDifReadonly = mapper.Map<PersonModelDifReadonly>(person);

Console.WriteLine($"PersonModel            => {personModel.Id}: {personModel.Name}");
Console.WriteLine($"PersonModelReadOnly    => {personModel.Id}: {personModel.Name}");
Console.WriteLine($"PersonModelDif         => {personModelDif.Codigo}: {personModelDif.Nome}");
Console.WriteLine($"PersonModelDifReadOnly => {personModelDifReadonly.Codigo}: {personModelDifReadonly.Nome}");

// Mapear por cima de uma instância
var person2 = new Person(
    id: Guid.Parse("85246202-282b-48cc-bea8-bab1447f07df"),
    name: "Fulano de tal",
    birthDate: DateTime.Now);

mapper.Map(person2, personModel);
mapper.Map(person2, personModelDif);

Console.WriteLine($"[person2] PersonModel    => {personModel.Id}: {personModel.Name}");
Console.WriteLine($"[person2] PersonModelDif => {personModelDif.Codigo}: {personModelDif.Nome}");