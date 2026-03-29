using AgentAI.Domain.Interfaces;

namespace AgentAI.Domain.Entities;

public class MemoryItem : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string? RepositoryPath { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public User? User { get; set; }
}
