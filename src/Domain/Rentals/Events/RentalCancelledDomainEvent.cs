using Domain.Abstractions;

namespace Domain.Rentals.Events;

public sealed record RentalCancelledDomainEvent(Guid Id) : IDomainEvent;