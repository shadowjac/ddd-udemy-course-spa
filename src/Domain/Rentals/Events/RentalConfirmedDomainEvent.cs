using Domain.Abstractions;

namespace Domain.Rentals.Events;

public sealed record RentalConfirmedDomainEvent(Guid Id) : IDomainEvent;