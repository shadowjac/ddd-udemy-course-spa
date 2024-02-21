using Domain.Abstractions;

namespace Domain.Rentals.Events;

public sealed record RentalCompletedDomainEvent(Guid Id) : IDomainEvent;