using Conductor.Engine.Domain.Organisation;
using Conductor.Engine.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conductor.Engine.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(b => b.FirstName).IsRequired();
        builder.Property(b => b.LastName).IsRequired();

        builder.Property(a => a.Id)
            .HasConversion(
                id => id.Value,
                value => new UserId(value)
            );

        builder.HasMany<Organisation>().WithMany();
    }
}