using AgentAI.Application.DTOs.Retrieval;
using AgentAI.Application.DTOs.Tool;
using AgentAI.Domain.Enums;

namespace AgentAI.Application.DTOs.Chat;

public class ChatResponse
{
    public Guid PromptRecordId { get; set; }
    public string Response { get; set; } = string.Empty;
    public IntentType DetectedIntent { get; set; }
    public int TokensUsed { get; set; }
    public int ContextChunksRetrieved { get; set; }
    public TimeSpan ResponseTime { get; set; }
    public List<RetrievalChunkDto>? RetrievedChunks { get; set; }
    public List<ToolExecutionDto>? SuggestedTools { get; set; }
}
