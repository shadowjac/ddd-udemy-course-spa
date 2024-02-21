namespace Domain.Vehicles.Repositories;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}