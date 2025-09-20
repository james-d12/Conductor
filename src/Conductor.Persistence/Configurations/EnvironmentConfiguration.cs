using Conductor.Core.Modules.Environment.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Environment = Conductor.Core.Modules.Environment.Domain.Environment;

namespace Conductor.Persistence.Configurations;

internal sealed class EnvironmentConfiguration : IEntityTypeConfiguration<Environment>
{
    public void Configure(EntityTypeBuilder<Environment> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(b => b.Name).IsRequired();
        builder.Property(b => b.Description).IsRequired();
        builder.Property(b => b.CreatedAt).IsRequired().HasDefaultValueSql("now()");
        builder.Property(b => b.UpdatedAt).IsRequired().HasDefaultValueSql("now()");

        builder.Property(r => r.Id)
            .HasConversion(
                id => id.Value,
                value => new EnvironmentId(value)
            );

        builder.HasMany(a => a.Deployments)
            .WithOne()
            .HasForeignKey(d => d.EnvironmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}