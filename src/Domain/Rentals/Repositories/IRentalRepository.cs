using Domain.Vehicles;

namespace Domain.Rentals.Repositories;

public interface IRentalRepository
{
    Task<Rental?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> IsOverlappingAsync(
        Vehicle vehicle,
        DateRange duration,
        CancellationToken cancellationToken = default);
    
    //add
    Task AddAsync(Rental rental, CancellationToken cancellationToken = default);
    
    
}