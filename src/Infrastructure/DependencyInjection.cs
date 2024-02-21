using Application.Abstractions.Clock;
using Application.Abstractions.Data;
using Application.Abstractions.Email;
using Dapper;
using Domain.Abstractions;
using Domain.Rentals.Repositories;
using Domain.Users.Repositories;
using Domain.Vehicles.Repositories;
using Infrastructure.Clock;
using Infrastructure.Data;
using Infrastructure.Email;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();

        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new ArgumentNullException(nameof(configuration), "Connection string not found");


        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

        return services;
    }
}