using AgentAI.Application.DTOs.Chat;
using AgentAI.Domain.Enums;

namespace AgentAI.Application.DTOs.Session;

public class SessionDetailDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public SessionStatus Status { get; set; }
    public string? RepositoryPath { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime LastActivityAt { get; set; }
    public int MessageCount { get; set; }
    public List<MessageDto> Messages { get; set; } = new();
}
