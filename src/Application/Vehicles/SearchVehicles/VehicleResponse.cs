namespace Application.Vehicles.SearchVehicles;

public sealed class VehicleResponse
{
    public Guid Id { get; init; }
    public string? Vin { get; init; }
    public string? Model { get; init; }
    public decimal Price { get; init; }
    public string? Currency { get; init; }
    public AddressResponse? Address { get; set; }
}