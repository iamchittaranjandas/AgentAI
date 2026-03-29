using AgentAI.Domain.Interfaces;

namespace AgentAI.Domain.Entities;

public class Repository : IAuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string? GitUrl { get; set; }
    public string? DefaultBranch { get; set; } = "main";
    public DateTime? LastIndexed { get; set; }
    public bool IsActive { get; set; } = true;
    public int TotalFiles { get; set; }
    public int IndexedFiles { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public ICollection<CodeFile> CodeFiles { get; set; } = new List<CodeFile>();
}
