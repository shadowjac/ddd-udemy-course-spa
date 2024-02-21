using FluentValidation;

namespace Application.Rentals.BookRental;

internal class BookRentalCommandValidator : AbstractValidator<BookRentalCommand>
{
    public BookRentalCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty();
        RuleFor(c => c.VehicleId)
            .NotEmpty();
        RuleFor(c => c.StartDate)
            .LessThan(c => c.EndDate);
    }
}