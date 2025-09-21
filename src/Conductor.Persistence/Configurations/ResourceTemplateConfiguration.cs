using Conductor.Core.ResourceTemplate.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conductor.Persistence.Configurations;

internal sealed class ResourceTemplateConfiguration : IEntityTypeConfiguration<ResourceTemplate>
{
    public void Configure(EntityTypeBuilder<ResourceTemplate> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasIndex(r => r.Name).IsUnique();

        builder.Property(b => b.Name).IsRequired();
        builder.Property(b => b.Type).IsRequired();
        builder.Property(b => b.Description).IsRequired();
        builder.Property(b => b.Provider).IsRequired().HasConversion<string>();
        builder.Property(b => b.CreatedAt).IsRequired().HasDefaultValueSql("now()");
        builder.Property(b => b.UpdatedAt).IsRequired().HasDefaultValueSql("now()");

        builder.Property(r => r.Id)
            .HasConversion(
                id => id.Value,
                value => new ResourceTemplateId(value)
            );

        builder.OwnsMany(r => r.Versions, v =>
        {
            v.WithOwner().HasForeignKey("TemplateId");
            v.HasKey("TemplateId", "Version");

            v.Property(x => x.Version).IsRequired();
            v.Property(x => x.Notes).IsRequired();
            v.Property(x => x.State).IsRequired().HasConversion<int>();
            v.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("now()");

            v.OwnsOne(r => r.Source, s =>
            {
                s.Property(p => p.BaseUrl)
                    .IsRequired()
                    .HasConversion(
                        uri => uri.ToString(),
                        str => new Uri(str)
                    );

                s.Property(p => p.FolderPath).IsRequired();
            });
        });
    }
}