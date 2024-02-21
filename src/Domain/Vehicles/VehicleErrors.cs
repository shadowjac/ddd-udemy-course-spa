using Domain.Abstractions;

namespace Domain.Vehicles;

public static class VehicleErrors
{
    public static Error NotFound => new
    ("Vehicle.NotFound",
        "Vehicle not found");
}