using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using nhibernate_playground;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

ISessionFactory CreateSessionFactory()
{
    var connectionString = "Server=localhost;Port=5432;Database=sample_nhibernate;User Id=postgres;Password=123456;";

    // var autoMap = AutoMap.AssemblyOf<Pessoa>()
    //     .Where(t => typeof(Pessoa).IsAssignableFrom(t));

    return Fluently.Configure()
        .Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString(connectionString))
        // .Mappings(m => m.AutoMappings.Add(autoMap))
        .Mappings(m => m.FluentMappings.Add<PessoaMap>())
        .Mappings(m => m.FluentMappings.Add<CidadeMap>())
        .ExposeConfiguration(TreatConfiguration)
        .BuildSessionFactory();
}

void TreatConfiguration(Configuration configuration)
{
    configuration.DataBaseIntegration(x =>
    {
        x.LogSqlInConsole = true;
        x.LogFormattedSql = true;
    });
    var update = new SchemaUpdate(configuration);
    update.Execute(false, true);
}

Console.WriteLine(".:: NHibernate Sample ::.");

var SampleId1 = Guid.Parse("903460bd-f6cf-4aeb-893a-f9c7dc8af717");
var SampleId2 = Guid.Parse("978ac6a9-4aa1-4340-8805-f468a3994441");

var factory = CreateSessionFactory();
var session = factory.OpenSession();

// Inicializando cidade
session.SaveOrUpdate(new Cidade(1, "São Paulo"));
session.SaveOrUpdate(new Cidade(2, "Florianópolis"));
session.Flush();

// Get by id
var pessoa1 = session.Get<Pessoa>(SampleId1);
if (pessoa1 == null)
{
    pessoa1 = new Pessoa(SampleId1, "Teste 1", "Teste 1 apelido", session.Get<Cidade>(1));
    session.Save(pessoa1);
    session.Flush();
}

var pessoa1_SecondLoad = session.Get<Pessoa>(SampleId1);

Console.WriteLine($"Same instance? = {pessoa1 == pessoa1_SecondLoad}"); // Verdadeiro

// Insert or update
var pessoa2 = session.Get<Pessoa>(SampleId2);
if (pessoa2 == null)
{
    pessoa2 = new Pessoa(SampleId2, "Teste 2", "Teste 2222", new Cidade(2, "Fake")); // Aqui a entidade de cidade não trackeada, não reflete em atualização do no DB
    session.Save(pessoa2);
}
else
{
    pessoa2.Nome = "Pessoa 2 atualizada";
    pessoa2.Cidade.Nome = "Fake update"; // Assim como no Entity Framework, a entidade Cidade é atualizada com o novo nome
    session.Update(pessoa2);
}
session.Flush();

var teste = session.Query<Pessoa>().ToArray();
Console.WriteLine($"Same instance of queryable? = {pessoa1 == teste.FirstOrDefault(x => x.Id == SampleId1)}"); // Verdadeiro

// Query 1
session.Dispose();
session = factory.OpenSession();
var queryResult1 = session.Query<Pessoa>().Where(x => x.Nome != null).ToArray(); // Query N+1 para carregamento da entidade Cidade. Se o Id da entidade relacionada já estiver na memória do Session do NHibernate, é reutilizada a instância sem fazer o sql N+1

foreach (var pessoa in queryResult1)
    Console.WriteLine($"| {pessoa.Id} | {pessoa.Nome,-20} | {pessoa.Apelido,-20} | {pessoa.Cidade.Nome,-20} |");

// Query 2
session.Dispose();
session = factory.OpenSession();
var queryResult2 = session.Query<Pessoa>()
    .Where(x => x.Nome != null)
    .Select(x => new
    {
        x.Id,
        x.Nome,
        CidadeId = x.Cidade.Id,
        NomeCidade = x.Cidade.Nome
    }).ToArray(); // SQL gerado com left join
foreach (var pessoa in queryResult2)
    Console.WriteLine($"| {pessoa.Id} | {pessoa.Nome,-20} | {pessoa.CidadeId,5} | {pessoa.NomeCidade,-20} |");

// Query 3
session.Dispose();
session = factory.OpenSession();
var queryResult3 = session.QueryOver<Pessoa>()
    .Where(x => x.Nome != null)
    .Inner.JoinQueryOver(x => x.Cidade)
    .List(); // SQL gerado com inner join

foreach (var pessoa in queryResult3)
    Console.WriteLine($"| {pessoa.Id} | {pessoa.Nome,-20} | {pessoa.Apelido,-20} | {pessoa.Cidade.Nome,-20} |");

Console.WriteLine($"Same instance of queryable? = {pessoa1 == queryResult1.FirstOrDefault(x => x.Id == SampleId1)}"); // Verdadeiro

Console.WriteLine("Fim");
