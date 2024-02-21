using Domain.Rentals;
using Domain.Reviews;
using Domain.Users;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");

        builder.HasKey(review => review.Id);

        builder.Property(review => review.Rating)
            .HasConversion(rating => rating.Value,
                value => Rating.Create(value).Value);

        builder.Property(review => review.Comment)
            .HasMaxLength(200)
            .HasConversion(comment => comment.Value,
                value => new Comment(value));

        builder.HasOne<Vehicle>()
            .WithMany()
            .HasForeignKey(review => review.VehicleId);

        builder.HasOne<Rental>()
            .WithMany()
            .HasForeignKey(review => review.RentalId);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(review => review.UserId);
    }
}