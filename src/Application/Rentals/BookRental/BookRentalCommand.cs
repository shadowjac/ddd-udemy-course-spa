using Application.Abstractions.Messaging;

namespace Application.Rentals.BookRental;

public sealed record BookRentalCommand(
    Guid VehicleId,
    Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate) : ICommand<Guid>;