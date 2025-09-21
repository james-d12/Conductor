using Conductor.Core.Application.Domain;
using Conductor.Core.Deployment.Domain;
using Conductor.Core.ResourceTemplate.Domain;
using Microsoft.EntityFrameworkCore;
using Environment = Conductor.Core.Environment.Domain.Environment;

namespace Conductor.Persistence;

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