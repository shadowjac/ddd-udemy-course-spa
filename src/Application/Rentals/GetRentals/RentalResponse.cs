namespace Application.Rentals.GetRentals;

public sealed class RentalResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid VehicleId { get; init; }
    public int Status { get; init; }
    public decimal RentalPrice { get; set; }
    public string? Currency { get; set; }
    public decimal MaintenancePrice { get; init; }
    public string? MaintenanceCurrency { get; init; }
    public decimal AccessoriesPrice { get; init; }
    public string? AccessoriesCurrency { get; init; }
    public decimal TotalPrice { get; init; }
    public string? TotalPriceCurrency { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public DateTime CreatedAt { get; init; }
}