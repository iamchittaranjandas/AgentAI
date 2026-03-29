using AgentAI.Application.Common.Models;
using AgentAI.Application.DTOs.Session;
using MediatR;

namespace AgentAI.Application.Features.Sessions.Queries;

public class GetSessionByIdQuery : IRequest<Result<SessionDto>>
{
    public Guid SessionId { get; set; }

    public GetSessionByIdQuery(Guid sessionId)
    {
        SessionId = sessionId;
    }
}
