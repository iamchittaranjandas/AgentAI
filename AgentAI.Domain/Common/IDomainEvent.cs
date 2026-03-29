using MediatR;

namespace AgentAI.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
