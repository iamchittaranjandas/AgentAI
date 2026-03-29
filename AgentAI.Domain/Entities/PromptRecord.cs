using AgentAI.Domain.Enums;
using AgentAI.Domain.Interfaces;

namespace AgentAI.Domain.Entities;

public class PromptRecord : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public string UserPrompt { get; set; } = string.Empty;
    public IntentType DetectedIntent { get; set; }
    public string? AssistantResponse { get; set; }
    public int TokensUsed { get; set; }
    public int ContextChunksRetrieved { get; set; }
    public TimeSpan ResponseTime { get; set; }
    public bool WasSuccessful { get; set; } = true;
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public Session Session { get; set; } = null!;
    public ICollection<RetrievalChunk> RetrievedChunks { get; set; } = new List<RetrievalChunk>();
}
