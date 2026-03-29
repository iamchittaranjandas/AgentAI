using AgentAI.Domain.Enums;
using AgentAI.Domain.Interfaces;

namespace AgentAI.Domain.Entities;

public class User : IAuditableEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public ICollection<Session> Sessions { get; set; } = new List<Session>();
    public ICollection<MemoryItem> MemoryItems { get; set; } = new List<MemoryItem>();
}
