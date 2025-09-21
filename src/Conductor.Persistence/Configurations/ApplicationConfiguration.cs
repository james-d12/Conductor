using Conductor.Core.Application.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApplicationId = Conductor.Core.Application.Domain.ApplicationId;

namespace Conductor.Persistence.Configurations;

internal sealed class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(b => b.Name).IsRequired();
        builder.Property(b => b.CreatedAt).IsRequired().HasDefaultValueSql("now()");
        builder.Property(b => b.UpdatedAt).IsRequired().HasDefaultValueSql("now()");

        builder.Property(a => a.Id)
            .HasConversion(
                id => id.Value,
                value => new ApplicationId(value)
            );

        builder.HasMany(a => a.Deployments)
            .WithOne()
            .HasForeignKey(d => d.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.OwnsOne(a => a.Repository, r =>
        {
            r.Property(x => x.Id).IsRequired();
            r.Property(x => x.Name).IsRequired();
            r.Property(x => x.Url).IsRequired();
            r.Property(x => x.Provider).IsRequired().HasConversion<string>();
        });

    }
}