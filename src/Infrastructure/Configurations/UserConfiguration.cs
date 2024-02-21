using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Name)
            .HasMaxLength(200)
            .HasConversion(name => name!.Value,
                value => new Name(value));

        builder.Property(user => user.LastName)
            .HasMaxLength(200)
            .HasConversion(lastName => lastName!.Value,
                value => new LastName(value));

        builder.Property(user => user.Email)
            .HasMaxLength(400)
            .HasConversion(email => email!.Value,
                value => new Domain.Users.Email(value));
        
        builder.HasIndex(user => user.Email)
            .IsUnique();
    }
}