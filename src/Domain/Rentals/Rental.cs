using Domain.Abstractions;
using Domain.Rentals.Events;
using Domain.Shared;
using Domain.Vehicles;

namespace Domain.Rentals
{
    public sealed class Rental : Entity
    {
        private Rental()
        {
        }

        private Rental(Guid id,
            RentalStatus rentalStatus,
            DateRange duration,
            Guid vehicleId,
            Guid userId,
            Money? pricePerRental,
            Money? maintenance,
            Money? accessories,
            Money? totalPrice,
            DateTime createdAt) : base(id)
        {
            RentalStatus = rentalStatus;
            Duration = duration;
            VehicleId = vehicleId;
            UserId = userId;
            PricePerRental = pricePerRental;
            Maintenance = maintenance;
            Accessories = accessories;
            TotalPrice = totalPrice;
            CreatedAt = createdAt;
        }

        public RentalStatus RentalStatus { get; private set; }
        public DateRange Duration { get; private set; }
        public Guid VehicleId { get; private set; }
        public Guid UserId { get; private set; }
        public Money? PricePerRental { get; private set; }
        public Money? Maintenance { get; private set; }
        public Money? Accessories { get; private set; }
        public Money? TotalPrice { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ConfirmedAt { get; private set; }
        public DateTime? CancelledAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public DateTime? DeniedAt { get; private set; }

        public static Rental Book(
            Vehicle vehicle,
            Guid userId,
            DateRange duration,
            PriceService priceService,
            DateTime createdAt)
        {
            var priceDetail = priceService.CalculatePrice(vehicle, duration);

            var rental = new Rental(
                Guid.NewGuid(),
                RentalStatus.Reserved,
                duration,
                vehicle.Id,
                userId,
                priceDetail.PricePerRental,
                priceDetail.Maintenance,
                priceDetail.Accessories,
                priceDetail.TotalPrice,
                createdAt
            );

            rental.RaiseDomainEvent(new RentalBookedDomainEvent(rental.Id!));
            vehicle.LastRentalDate = createdAt;

            return rental;
        }

        public Result Confirm(DateTime utcNow)
        {
            if (RentalStatus != RentalStatus.Reserved)
            {
                return Result.Failure(RentalErrors.NotReserved);
            }

            RentalStatus = RentalStatus.Confirmed;
            ConfirmedAt = utcNow;
            RaiseDomainEvent(new RentalConfirmedDomainEvent(Id!));

            return Result.Success();
        }

        //reject booking
        public Result Reject(DateTime utcNow)
        {
            if (RentalStatus != RentalStatus.Reserved)
            {
                return Result.Failure(RentalErrors.NotReserved);
            }

            RentalStatus = RentalStatus.Rejected;
            DeniedAt = utcNow;
            RaiseDomainEvent(new RentalDeniedDomainEvent(Id!));

            return Result.Success();
        }

        // cancel booking
        public Result Cancel(DateTime utcNow)
        {
            if (RentalStatus != RentalStatus.Confirmed)
            {
                return Result.Failure(RentalErrors.NotConfirmed);
            }

            var currentDate = DateOnly.FromDateTime(utcNow);
            if (currentDate > Duration!.StartDate)
            {
                return Result.Failure(RentalErrors.AlreadyStarted);
            }

            RentalStatus = RentalStatus.Cancelled;
            CancelledAt = utcNow;
            RaiseDomainEvent(new RentalCancelledDomainEvent(Id!));

            return Result.Success();
        }

        public Result Complete(DateTime utcNow)
        {
            if (RentalStatus != RentalStatus.Confirmed)
            {
                return Result.Failure(RentalErrors.NotConfirmed);
            }

            RentalStatus = RentalStatus.Completed;
            CompletedAt = utcNow;
            RaiseDomainEvent(new RentalCompletedDomainEvent(Id!));

            return Result.Success();
        }
    }
}