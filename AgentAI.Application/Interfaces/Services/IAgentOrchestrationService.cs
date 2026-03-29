using AgentAI.Application.Common;
using AgentAI.Application.DTOs.Chat;

namespace AgentAI.Application.Interfaces.Services;

public interface IAgentOrchestrationService
{
    Task<Result<ChatResponse>> ProcessChatRequestAsync(ChatRequest request, CancellationToken cancellationToken = default);
    Task<Result<ChatResponse>> ProcessStreamingChatRequestAsync(ChatRequest request, CancellationToken cancellationToken = default);
}
