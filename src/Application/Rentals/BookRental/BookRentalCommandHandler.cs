using Application.Abstractions.Clock;
using Application.Abstractions.Messaging;
using Application.Exceptions;
using Domain.Abstractions;
using Domain.Rentals;
using Domain.Rentals.Repositories;
using Domain.Users;
using Domain.Users.Repositories;
using Domain.Vehicles;
using Domain.Vehicles.Repositories;

namespace Application.Rentals.BookRental;

internal sealed class BookRentalCommandHandler : ICommandHandler<BookRentalCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IRentalRepository _rentalRepository;
    private readonly PriceService _priceService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public BookRentalCommandHandler(IUserRepository userRepository,
        IVehicleRepository vehicleRepository,
        IRentalRepository rentalRepository,
        PriceService priceService,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _vehicleRepository = vehicleRepository;
        _rentalRepository = rentalRepository;
        _priceService = priceService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<Guid>> Handle(BookRentalCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }
        
        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);
        if (vehicle is null)
        {
            return Result.Failure<Guid>(VehicleErrors.NotFound);
        }

        var duration = DateRange.Create(request.StartDate, request.EndDate);

        if (await _rentalRepository.IsOverlappingAsync(vehicle, duration, cancellationToken))
        {
            return Result.Failure<Guid>(RentalErrors.Overlap);
        }

        try
        {
            var rental = Rental.Book(
                vehicle,
                user.Id,
                duration,
                _priceService,
                _dateTimeProvider.UtcNow);

            await _rentalRepository.AddAsync(rental, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return rental.Id;
        }
        catch (ConcurrencyException e)
        {
            return Result.Failure<Guid>(RentalErrors.Overlap);
        }
    }
}