using AgentAI.Domain.Common;

namespace AgentAI.Domain.Events;

public class SessionCreatedEvent : IDomainEvent
{
    public Guid SessionId { get; }
    public Guid UserId { get; }
    public string Title { get; }
    public DateTime OccurredOn { get; }

    public SessionCreatedEvent(Guid sessionId, Guid userId, string title)
    {
        SessionId = sessionId;
        UserId = userId;
        Title = title;
        OccurredOn = DateTime.UtcNow;
    }
}
