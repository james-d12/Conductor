using Conductor.Domain.Application.Domain;
using Conductor.Domain.Deployment.Domain;
using Conductor.Domain.Environment.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApplicationId = Conductor.Domain.Application.Domain.ApplicationId;
using Environment = Conductor.Domain.Environment.Domain.Environment;

namespace Conductor.Persistence.Configurations;

internal sealed class DeploymentConfiguration : IEntityTypeConfiguration<Deployment>
{
    public void Configure(EntityTypeBuilder<Deployment> builder)
    {
        builder.HasKey(d => d.Id);

        builder.HasIndex(d => new { d.ApplicationId, d.EnvironmentId, d.CommitId, d.Status })
            .IsUnique();

        builder.Property(d => d.Id)
            .HasConversion(
                id => id.Value,
                value => new DeploymentId(value)
            );

        builder.Property(d => d.ApplicationId)
            .HasConversion(
                id => id.Value,
                value => new ApplicationId(value)
            );

        builder.Property(d => d.EnvironmentId)
            .HasConversion(
                id => id.Value,
                value => new EnvironmentId(value)
            );

        builder.Property(d => d.CommitId)
            .HasConversion(
                id => id.Value,
                value => new CommitId(value)
            );

        builder.HasOne<Application>()
            .WithMany()
            .HasForeignKey(d => d.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Environment>()
            .WithMany()
            .HasForeignKey(d => d.EnvironmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(d => d.Status).HasConversion<string>();
        builder.Property(d => d.CreatedAt).IsRequired().HasDefaultValueSql("now()");
        builder.Property(d => d.UpdatedAt).IsRequired().HasDefaultValueSql("now()");
    }
}