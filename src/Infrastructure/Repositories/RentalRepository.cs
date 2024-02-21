using Domain.Rentals;
using Domain.Rentals.Repositories;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class RentalRepository : Repository<Rental>, IRentalRepository
{
    private static readonly RentalStatus[] RentalStatuses =
    [
        RentalStatus.Reserved,
        RentalStatus.Confirmed,
        RentalStatus.Completed
    ];

    public RentalRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> IsOverlappingAsync(Vehicle vehicle, DateRange duration,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Rental>()
            .AnyAsync(
                r => r.VehicleId == vehicle.Id &&
                     r.Duration.StartDate <= duration.EndDate &&
                     r.Duration.EndDate >= duration.StartDate &&
                     RentalStatuses.Contains(r.RentalStatus),
                cancellationToken
            );
    }
}