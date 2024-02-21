using Domain.Shared;
using Domain.Vehicles;

namespace Domain.Rentals;

public record PriceDetail(
    Money PricePerRental,
    Money Maintenance,
    Money Accessories,
    Money TotalPrice);