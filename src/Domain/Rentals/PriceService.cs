using Domain.Shared;
using Domain.Vehicles;

namespace Domain.Rentals;

public sealed class PriceService
{
    public PriceDetail CalculatePrice(Vehicle vehicle, DateRange dateRange)
    {
        var currency = vehicle.Price!.Currency;
        var pricePerRental = new Money(
            dateRange.NumberOfDays * vehicle.Price.Amount,
            currency);

        decimal percentageChange = 0;

        foreach (var accessory in vehicle.Accessories)
        {
            percentageChange += accessory switch
            {
                Accessories.AppleCar or Accessories.AndroidCar => 0.05M,
                Accessories.AirConditioner => 0.01M,
                Accessories.Maps => 0.01M,
                _ => 0
            };
        }

        var accessoriesCharges = Money.Zero(currency);
        if (percentageChange > 0)
        {
            accessoriesCharges = new Money(
                pricePerRental.Amount * percentageChange,
                currency);
        }

        var totalPrice = Money.Zero(currency);
        totalPrice += pricePerRental;

        if (!vehicle!.MaintenanceCost!.IsZero())
        {
            totalPrice += vehicle.MaintenanceCost;
        }

        totalPrice += accessoriesCharges;

        return new PriceDetail(
            pricePerRental,
            vehicle.MaintenanceCost,
            accessoriesCharges,
            totalPrice);
    }
}