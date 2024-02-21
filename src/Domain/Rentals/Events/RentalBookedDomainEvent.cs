using Domain.Abstractions;

namespace Domain.Rentals.Events;

public sealed record RentalBookedDomainEvent(Guid RentalId) : IDomainEvent;