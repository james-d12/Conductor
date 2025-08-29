using Conductor.Core.Modules.ResourceTemplate.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conductor.Persistence.Configurations;

internal sealed class ResourceTemplateConfiguration : IEntityTypeConfiguration<ResourceTemplate>
{
    public void Configure(EntityTypeBuilder<ResourceTemplate> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(b => b.Name).IsRequired();
        builder.Property(b => b.Description).IsRequired();
        builder.Property(b => b.Provider).IsRequired().HasConversion<string>();
        builder.Property(b => b.Type).IsRequired().HasConversion<string>();
        builder.Property(b => b.CreatedAt).IsRequired().HasDefaultValueSql("now()");
        builder.Property(b => b.UpdatedAt).IsRequired().HasDefaultValueSql("now()");

        builder.Property(r => r.Id)
            .HasConversion(
                id => id.Id,
                value => new ResourceTemplateId(value)
            );

        builder.OwnsMany(r => r.Versions, v =>
        {
            v.WithOwner().HasForeignKey("TemplateId");
            v.HasKey("TemplateId", "Version");

            v.Property(x => x.Version).IsRequired();
            v.Property(x => x.Source).IsRequired();
            v.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("now()");
            v.Property(x => x.Notes).IsRequired();
        });
    }
}