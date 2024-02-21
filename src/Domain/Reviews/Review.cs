using Domain.Abstractions;
using Domain.Rentals;
using Domain.Reviews.Events;

namespace Domain.Reviews;

public sealed class Review : Entity
{
    private Review()
    {
    }

    private Review(Guid id,
        Guid vehicleId,
        Guid rentalId,
        Guid userId,
        Rating rating,
        Comment comment,
        DateTime? createdAt) : base(id)
    {
        VehicleId = vehicleId;
        RentalId = rentalId;
        UserId = userId;
        Rating = rating;
        Comment = comment;
        CreatedAt = createdAt;
    }

    public Guid VehicleId { get; private set; }
    public Guid RentalId { get; private set; }
    public Guid UserId { get; private set; }
    public Rating Rating { get; private set; }
    public Comment Comment { get; private set; }
    public DateTime? CreatedAt { get; private set; }

    // create method returning result of review
    public static Result<Review> Create(
        Rental rental,
        Rating rating,
        Comment comment,
        DateTime createdAt)
    {
        if (rental.RentalStatus != RentalStatus.Completed)
        {
            return Result.Failure<Review>(ReviewErrors.NotEligible);
        }

        var review = new Review(
            Guid.NewGuid(),
            rental.VehicleId,
            rental.Id,
            rental.UserId,
            rating,
            comment,
            createdAt
        );

        review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.Id));

        return review;
    }
}