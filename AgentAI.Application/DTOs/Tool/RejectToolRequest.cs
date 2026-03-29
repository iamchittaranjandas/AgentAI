using System.ComponentModel.DataAnnotations;

namespace AgentAI.Application.DTOs.Tool;

public class RejectToolRequest
{
    [MaxLength(1000)]
    public string? Reason { get; set; }
}
