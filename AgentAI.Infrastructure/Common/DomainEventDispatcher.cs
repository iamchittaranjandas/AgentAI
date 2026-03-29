using AgentAI.Domain.Common;
using AgentAI.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AgentAI.Infrastructure.Common;

public static class DomainEventDispatcher
{
    public static async Task DispatchDomainEventsAsync(ApplicationDbContext context, IMediator mediator, CancellationToken cancellationToken = default)
    {
        var domainEntities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }
    }
}
