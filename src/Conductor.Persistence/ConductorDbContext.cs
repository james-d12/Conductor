using Conductor.Core.Modules.Application.Domain;
using Conductor.Core.Modules.ResourceTemplate.Domain;
using Microsoft.EntityFrameworkCore;
using Environment = Conductor.Core.Modules.Environment.Domain.Environment;

namespace Conductor.Persistence;

public sealed class ConductorDbContext : DbContext
{
    public required DbSet<ResourceTemplate> ResourceTemplates { get; init; }
    public required DbSet<Application> Applications { get; init; }
    public required DbSet<Environment> Environments { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(AppContext.BaseDirectory, "Conductor.db");

        if (!File.Exists(dbPath))
        {
            File.Create(dbPath);
        }

        optionsBuilder.UseSqlite("Data Source=" + dbPath);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConductorDbContext).Assembly);
    }
}