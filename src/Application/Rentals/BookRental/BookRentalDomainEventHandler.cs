using Application.Abstractions.Email;
using Domain.Rentals.Events;
using Domain.Rentals.Repositories;
using Domain.Users.Repositories;
using MediatR;

namespace Application.Rentals.BookRental;

internal sealed class BookRentalDomainEventHandler : INotificationHandler<RentalBookedDomainEvent>
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public BookRentalDomainEventHandler(IRentalRepository rentalRepository,
        IUserRepository userRepository,
        IEmailService emailService)
    {
        _rentalRepository = rentalRepository;
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task Handle(RentalBookedDomainEvent notification, CancellationToken cancellationToken)
    {
        var rental = await _rentalRepository.GetByIdAsync(notification.RentalId, cancellationToken);

        if (rental is null)
        {
            return;
        }

        var user = await _userRepository.GetByIdAsync(rental.UserId, cancellationToken);

        if (user is null)
        {
            return;
        }

        await _emailService.SendAsync(user.Email!, "Rental booked", "Please confirm this booking");
    }
}