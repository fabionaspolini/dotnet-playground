using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate_Sample;

ISessionFactory CreateSessionFactory()
{
    var connectionString = "Server=localhost;Port=5432;Database=sample_nhibernate;User Id=postgres;Password=123456;";

    // var autoMap = AutoMap.AssemblyOf<Pessoa>()
    //     .Where(t => typeof(Pessoa).IsAssignableFrom(t));

    return Fluently.Configure()
        .Database(
            PostgreSQLConfiguration.PostgreSQL82.ConnectionString(connectionString))
        // .Mappings(m => m.AutoMappings.Add(autoMap))
        .Mappings(m => m.FluentMappings.Add<PessoaMap>())
        .ExposeConfiguration(TreatConfiguration)
        .BuildSessionFactory();
}

void TreatConfiguration(Configuration configuration)
{
    var update = new SchemaUpdate(configuration);
    update.Execute(false, true);
}

Console.WriteLine(".:: NHibernate Sample ::.");

var SampleId1 = Guid.Parse("903460bd-f6cf-4aeb-893a-f9c7dc8af717");
var SampleId2 = Guid.Parse("978ac6a9-4aa1-4340-8805-f468a3994441");

var factory = CreateSessionFactory();
var session = factory.OpenSession();

// Get by id
var pessoa1 = session.Get<Pessoa>(SampleId1);
if (pessoa1 == null)
{
    pessoa1 = new Pessoa(SampleId1, "Teste 1", "Teste 1 apelido");
    session.Save(pessoa1);
    session.Flush();
}

var pessoa1_SecondLoad = session.Get<Pessoa>(SampleId1);

Console.WriteLine($"Same instance? = {pessoa1 == pessoa1_SecondLoad}"); // Verdadeiro

// Insert or update
var pessoa2 = session.Get<Pessoa>(SampleId2);
if (pessoa2 == null)
{
    pessoa2 = new Pessoa(SampleId2, "Teste 2", "Teste 2222");
    session.Save(pessoa2);
}
else
{
    pessoa2.Nome = "Pessoa 2 atualizada";
    session.Update(pessoa2);
}
session.Flush();

// Query
var query = session.Query<Pessoa>();
query = query.Where(x => x.Nome != null);
var pessoas = query.ToList();

foreach (var item in pessoas)
    Console.WriteLine($"| {item.Id} | {item.Nome, -20} | {item.Apelido,-20} |");

Console.WriteLine($"Same instance of queryable? = {pessoa1 == pessoas.FirstOrDefault(x => x.Id == SampleId1)}"); // Verdadeiro

Console.WriteLine("Fim");
