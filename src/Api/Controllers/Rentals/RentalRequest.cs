namespace Api.Controllers.Rentals;

public sealed record RentalRequest(
    Guid VehicleId,
    Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate);