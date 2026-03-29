using AgentAI.Application.Common.Models;
using AgentAI.Application.DTOs.Session;
using MediatR;

namespace AgentAI.Application.Features.Sessions.Commands;

public class CreateSessionCommand : IRequest<Result<SessionDto>>
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? RepositoryPath { get; set; }
    public string? Branch { get; set; }
}
