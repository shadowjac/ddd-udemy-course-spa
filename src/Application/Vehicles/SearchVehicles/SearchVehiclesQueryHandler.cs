using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Domain.Abstractions;
using Domain.Rentals;

namespace Application.Vehicles.SearchVehicles;

internal sealed class SearchVehiclesQueryHandler : IQueryHandler<SearchVehiclesQuery, IReadOnlyList<VehicleResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    private static readonly int[] ActiveRentalStatuses =
    [
        (int)RentalStatus.Reserved,
        (int)RentalStatus.Confirmed,
        (int)RentalStatus.Completed
    ];

    public SearchVehiclesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<VehicleResponse>>> Handle(SearchVehiclesQuery request,
        CancellationToken cancellationToken)
    {
        if (request.StartDate > request.EndDate)
        {
            return new List<VehicleResponse>().AsReadOnly();
        }

        using var connection = _sqlConnectionFactory.CreateConnection();
        const string sql = """
                               SELECT
                               v.id as Id,
                               v.model AS Model,
                               v.vin as Vin,
                               v.price_amount as Price,
                               v.price_currency as Currency,
                               v.address_country as Country,
                               v.address_state as State,
                               v.address_city as City,
                               v.address_street as Street
                               
                               
                           FROM public.vehicles AS v
                           WHERE NOT EXISTS
                           (
                               SELECT 1
                               FROM public.rentals AS r
                               WHERE r.vehicle_id = v.id AND
                                    r.duration_start_date <= @EndDate AND
                                    r.duration_end_date >= @StartDate AND
                                    r.rental_status = ANY(@ActiveRentalStatuses)
                                )
                           """;

        var vehicles = await connection
            .QueryAsync<VehicleResponse, AddressResponse, VehicleResponse>
            (
                sql,
                (vehicle, address) =>
                {
                    vehicle.Address = address;
                    return vehicle;
                },
                new
                {
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    ActiveRentalStatuses = ActiveRentalStatuses
                },
                splitOn: "Country"
            );

        return vehicles.ToList().AsReadOnly();
    }
}