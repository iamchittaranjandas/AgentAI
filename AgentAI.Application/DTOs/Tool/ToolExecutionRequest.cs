using System.ComponentModel.DataAnnotations;
using AgentAI.Domain.Enums;

namespace AgentAI.Application.DTOs.Tool;

public class ToolExecutionRequest
{
    [Required]
    public Guid SessionId { get; set; }

    [Required]
    public ToolType ToolType { get; set; }

    [Required]
    [MaxLength(200)]
    public string Action { get; set; } = string.Empty;

    [Required]
    public Dictionary<string, object> Parameters { get; set; } = new();

    public bool AutoApprove { get; set; }
}
