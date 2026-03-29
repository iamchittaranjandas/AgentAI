using AgentAI.Domain.Enums;

namespace AgentAI.Application.DTOs.Session;

public class SessionDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public SessionStatus Status { get; set; }
    public string? RepositoryPath { get; set; }
    public string? Branch { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime LastActivityAt { get; set; }
    public int MessageCount { get; set; }
}
