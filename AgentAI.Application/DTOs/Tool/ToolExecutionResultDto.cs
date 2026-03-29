using AgentAI.Domain.Enums;

namespace AgentAI.Application.DTOs.Tool;

public class ToolExecutionResultDto
{
    public Guid ExecutionId { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; }
    public RiskLevel RiskLevel { get; set; }
    public bool RequiresApproval { get; set; }
    public string? Output { get; set; }
    public bool WasSuccessful { get; set; }
    public string? ErrorMessage { get; set; }
    public TimeSpan? ExecutionDuration { get; set; }
}
