using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Domain.Abstractions;

namespace Application.Rentals.GetRentals;

internal sealed class GetRentalQueryHandler : IQueryHandler<GetRentalsQuery, RentalResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetRentalQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    
    public async Task<Result<RentalResponse>> Handle(GetRentalsQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        const string sql = """
                           SELECT
                               r.id as Id,
                               r.vehicle_id as VehicleId,
                               r.status as Status,
                               r.price per_period as RentalPrice,
                               r.price_currency as Currency,
                               r.maintenance_price as MaintenancePrice,
                               r.maintenance_currency as MaintenanceCurrency,
                               r.accessories_price as AccessoriesPrice,
                               r.accessories_currency as AccessoriesCurrency,
                               r.total_price as TotalPrice,
                               r.total_price_currency as TotalPriceCurrency,
                               r.start_date as StartDate,
                               r.end_date as EndDate,
                               r.createdAt as CreatedAt
                           FROM  rentals r
                           WHERE r.id = @RentalId
                           """;
        var rental = await connection.QuerySingleOrDefaultAsync<RentalResponse>(
            sql,
            new
            {
                request.RentalId
            });

        return rental!;
    }
}