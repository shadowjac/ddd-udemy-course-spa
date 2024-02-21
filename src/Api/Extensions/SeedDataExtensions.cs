using Application.Abstractions.Data;
using Bogus;
using Dapper;
using Domain.Vehicles;

namespace Api.Extensions;

public static class SeedDataExtensions
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        using var connection = sqlConnectionFactory.CreateConnection();

        var faker = new Faker();
        var vehicles = new List<object>();

        for (var i = 0; i < 100; i++)
        {
            vehicles.Add(new
            {
                Id = Guid.NewGuid(),
                Vin = faker.Vehicle.Vin(),
                Model = faker.Vehicle.Model(),
                Country = faker.Address.Country(),
                State = faker.Address.State(),
                City = faker.Address.City(),
                Street = faker.Address.StreetAddress(),
                PriceAmount = faker.Random.Decimal(1000, 2000),
                PriceCurrency = "USD",
                MaintenancePrice = faker.Random.Decimal(100, 200),
                MaintenancePriceCurrency = "USD",
                Accessories = new List<int>()
                {
                    (int)Accessories.Wifi, (int)Accessories.AppleCar
                },
                LastRentalDate = DateTime.MinValue
            });
        }

        var sql = """
                  insert into public.vehicles (id, model, vin, address_country, address_state, address_city, address_street, price_amount,
                                        price_currency, maintenance_cost_amount, maintenance_cost_currency, last_rental_date, accessories)
                  values (@Id, @Model, @Vin, @Country, @State, @City, @Street, @PriceAmount, @PriceCurrency, @MaintenancePrice, @MaintenancePriceCurrency, @LastRentalDate, @Accessories );
                  """;

        connection.Execute(sql, vehicles);
    }
}