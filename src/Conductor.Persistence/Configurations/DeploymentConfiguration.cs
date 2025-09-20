using Conductor.Core.Modules.Deployment.Domain;
using Conductor.Core.Modules.Environment.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApplicationId = Conductor.Core.Modules.Application.Domain.ApplicationId;

namespace Conductor.Persistence.Configurations;

internal sealed class DeploymentConfiguration : IEntityTypeConfiguration<Deployment>
{
    public void Configure(EntityTypeBuilder<Deployment> builder)
    {
        builder.HasKey(d => d.Id);

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

        builder.Property(d => d.Status).HasConversion<string>();
        builder.Property(d => d.CreatedAt).IsRequired().HasDefaultValueSql("now()");
        builder.Property(d => d.UpdatedAt).IsRequired().HasDefaultValueSql("now()");
    }
}