using AgentAI.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AgentAI.Application.Features.Sessions.EventHandlers;

public class SessionCreatedEventHandler : INotificationHandler<SessionCreatedEvent>
{
    private readonly ILogger<SessionCreatedEventHandler> _logger;

    public SessionCreatedEventHandler(ILogger<SessionCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SessionCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Session created: {SessionId} for User: {UserId} with Title: {Title}",
            notification.SessionId,
            notification.UserId,
            notification.Title);

        return Task.CompletedTask;
    }
}
