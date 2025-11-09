using Conductor.Engine.Domain.Application;
using Conductor.Engine.Domain.Deployment;
using Conductor.Engine.Domain.ResourceTemplate;
using Microsoft.EntityFrameworkCore;
using Environment = Conductor.Engine.Domain.Environment.Environment;

namespace Conductor.Engine.Persistence;

public sealed class ConductorDbContext : DbContext
{
    public required DbSet<ResourceTemplate> ResourceTemplates { get; init; }
    public required DbSet<Application> Applications { get; init; }
    public required DbSet<Environment> Environments { get; init; }
    public required DbSet<Deployment> Deployments { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(Path.GetTempPath(), "Conductor.db");

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