using Domain.Vehicles;
using Domain.Vehicles.Repositories;

namespace Infrastructure.Repositories;

internal sealed class VehicleRepository(ApplicationDbContext dbContext)
    : Repository<Vehicle>(dbContext), IVehicleRepository;