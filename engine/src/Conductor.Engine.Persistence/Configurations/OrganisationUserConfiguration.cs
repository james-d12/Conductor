using Conductor.Engine.Domain.Organisation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conductor.Engine.Persistence.Configurations;

internal sealed class OrganisationUserConfiguration : IEntityTypeConfiguration<OrganisationUser>
{
    public void Configure(EntityTypeBuilder<OrganisationUser> builder)
    {
        builder.ToTable("OrganisationUsers");

        builder.HasKey(x => x.IdentityUserId);

        builder.Property(x => x.OrganisationId)
            .HasConversion(
                id => id.Value,
                value => new OrganisationId(value)
            );

        builder.HasOne<Organisation>()
            .WithMany()
            .HasForeignKey(x => x.OrganisationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<IdentityUser>()
            .WithMany()
            .HasForeignKey(x => x.IdentityUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}