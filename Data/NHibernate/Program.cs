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
            PostgreSQLConfiguration.Standard.ConnectionString(connectionString))
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

var factory = CreateSessionFactory();
var session = factory.OpenSession();

var pessoa1 = session.Get<Pessoa>(1);
if (pessoa1 == null)
{
    pessoa1 = new Pessoa
    {
        Nome = "Teste 1",
        Apelido = "Teste 1"
    };
    session.Save(pessoa1);
}

var pessoa2 = session.Get<Pessoa>(1);

Console.WriteLine($"Same instance? = {pessoa1 == pessoa2}");

// Query
var query = session.Query<Pessoa>();
query = query.Where(x => x.Id > 0);
var pessoas = query.ToList();

foreach (var item in pessoas)
    Console.WriteLine($"| {item.Id} | {item.Nome, -20} | {item.Apelido,-20} |");

Console.WriteLine($"Same instance of queryable? = {pessoa1 == pessoas.FirstOrDefault(x => x.Id == 1)}");

Console.WriteLine("Fim");
