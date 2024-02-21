using Domain.Rentals;
using Domain.Shared;
using Domain.Users;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal sealed class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.ToTable("rentals");

        builder.HasKey(rental => rental.Id);

        builder.OwnsOne(rental => rental.PricePerRental, pb =>
        {
            pb.Property(money => money.Currency)
                .HasConversion(currency => currency.Code,
                    code => Currency.From(code));
        });

        builder.OwnsOne(rental => rental.Maintenance, pb =>
        {
            pb.Property(money => money.Currency)
                .HasConversion(currency => currency.Code,
                    code => Currency.From(code));
        });

        builder.OwnsOne(rental => rental.Accessories, pb =>
        {
            pb.Property(money => money.Currency)
                .HasConversion(currency => currency.Code,
                    code => Currency.From(code));
        });

        builder.OwnsOne(rental => rental.TotalPrice, pb =>
        {
            pb.Property(money => money.Currency)
                .HasConversion(currency => currency.Code,
                    code => Currency.From(code));
        });

        builder.OwnsOne(rental => rental.Duration);

        builder.HasOne<Vehicle>()
            .WithMany()
            .HasForeignKey(rental => rental.VehicleId);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(rental => rental.UserId);
    }
}