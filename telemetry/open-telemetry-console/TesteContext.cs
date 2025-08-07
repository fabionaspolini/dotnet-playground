using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace open_telemetry_console_playground;

internal class TesteContext : DbContext
{
    public TesteContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Pessoa> Pessoas { get; private set; }
    public DbSet<PessoaSimulateException> PessoasSimulateException { get; private set; }
}

[Table("pessoa")]
record class Pessoa(int id, string nome);


[Table("pessoa_aaaaa")]
record class PessoaSimulateException(int id, string nome);