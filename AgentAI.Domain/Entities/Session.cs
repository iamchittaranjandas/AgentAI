using AgentAI.Domain.Enums;
using AgentAI.Domain.Interfaces;

namespace AgentAI.Domain.Entities;

public class Session : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public SessionStatus Status { get; set; }
    public string? RepositoryPath { get; set; }
    public string? Branch { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public DateTime LastActivityAt { get; set; }
    public int MessageCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public User User { get; set; } = null!;
    public ICollection<PromptRecord> PromptRecords { get; set; } = new List<PromptRecord>();
    public ICollection<ToolExecution> ToolExecutions { get; set; } = new List<ToolExecution>();
}
