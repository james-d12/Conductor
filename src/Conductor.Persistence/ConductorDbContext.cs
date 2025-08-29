using Conductor.Core.Modules.ResourceTemplate.Domain;
using Microsoft.EntityFrameworkCore;

namespace Conductor.Persistence;

public sealed class ConductorDbContext : DbContext
{
    public DbSet<ResourceTemplate> ResourceTemplates { get; init; }

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