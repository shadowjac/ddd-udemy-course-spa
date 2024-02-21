using Application.Abstractions.Messaging;

namespace Application.Vehicles.SearchVehicles;

public record SearchVehiclesQuery (
    DateOnly StartDate,
    DateOnly EndDate,
    string? Test) :IQuery<IReadOnlyList<VehicleResponse>>;