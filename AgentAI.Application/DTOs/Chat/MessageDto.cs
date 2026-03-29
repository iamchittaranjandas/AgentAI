using AgentAI.Domain.Enums;

namespace AgentAI.Application.DTOs.Chat;

public class MessageDto
{
    public Guid Id { get; set; }
    public string UserPrompt { get; set; } = string.Empty;
    public string? AssistantResponse { get; set; }
    public IntentType DetectedIntent { get; set; }
    public int TokensUsed { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool WasSuccessful { get; set; }
}
