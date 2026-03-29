using AgentAI.Domain.Enums;

namespace AgentAI.Application.DTOs.Tool;

public class ToolExecutionDto
{
    public Guid Id { get; set; }
    public ToolType ToolType { get; set; }
    public string ToolName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public ApprovalStatus ApprovalStatus { get; set; }
    public RiskLevel RiskLevel { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? ExecutedAt { get; set; }
    public bool WasSuccessful { get; set; }
}
