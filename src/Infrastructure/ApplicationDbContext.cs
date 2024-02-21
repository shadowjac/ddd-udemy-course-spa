using Application.Exceptions;
using Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    private readonly IPublisher _publisher;

    public ApplicationDbContext(DbContextOptions options, IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            await PublishDomainEventsAsync(cancellationToken);
            return result;
        }
        catch (DbUpdateConcurrencyException e)
        {
            throw new ConcurrencyException(e.Message, e);
        }
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<Entity>()
            .Select(x => x.Entity)
            .Where(x => x.DomainEvents.Any())
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents;
                entity.ClearDomainEvents();
                return domainEvents;
            }).ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        await base.SaveChangesAsync(cancellationToken);
    }
}