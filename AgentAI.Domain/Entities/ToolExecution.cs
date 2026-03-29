using AgentAI.Domain.Enums;
using AgentAI.Domain.Interfaces;

namespace AgentAI.Domain.Entities;

public class ToolExecution : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public Guid? PromptRecordId { get; set; }
    public ToolType ToolType { get; set; }
    public string ToolName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string InputParameters { get; set; } = string.Empty;
    public string? Output { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; }
    public RiskLevel RiskLevel { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? ExecutedAt { get; set; }
    public TimeSpan? ExecutionDuration { get; set; }
    public bool WasSuccessful { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public Session Session { get; set; } = null!;
    public PromptRecord? PromptRecord { get; set; }
}
