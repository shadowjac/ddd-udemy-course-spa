using Application.Abstractions.Messaging;

namespace Application.Rentals.GetRentals;

public sealed record GetRentalsQuery(Guid RentalId) : IQuery<RentalResponse>;