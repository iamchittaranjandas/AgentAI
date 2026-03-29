using System.ComponentModel.DataAnnotations;
using AgentAI.Domain.Enums;

namespace AgentAI.Application.DTOs.Session;

public class UpdateSessionRequest
{
    [MaxLength(500)]
    public string? Title { get; set; }

    public SessionStatus? Status { get; set; }
}
