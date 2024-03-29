﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EFCore_ZExtensionsPlayground.Infra;

/// <summary>
/// Fábrica para suportar CLI para criar e aplicar migrations, não utilizada em tempo de execução
/// </summary>
public class SampleContextDesignFactory : IDesignTimeDbContextFactory<SampleContext>
{
    public SampleContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SampleContext>();
        optionsBuilder.UseNpgsql(Program.ConnectioString);
        return new SampleContext(optionsBuilder.Options);
    }
}