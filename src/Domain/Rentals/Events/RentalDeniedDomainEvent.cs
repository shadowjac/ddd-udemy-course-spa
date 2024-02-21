using Domain.Abstractions;

namespace Domain.Rentals.Events;

public sealed record RentalDeniedDomainEvent(Guid Id) : IDomainEvent;