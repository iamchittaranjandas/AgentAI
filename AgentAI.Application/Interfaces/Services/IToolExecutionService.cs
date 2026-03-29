using AgentAI.Application.Common;
using AgentAI.Application.DTOs.Tool;

namespace AgentAI.Application.Interfaces.Services;

public interface IToolExecutionService
{
    Task<Result<ToolExecutionResultDto>> ExecuteToolAsync(ToolExecutionRequest request, CancellationToken cancellationToken = default);
    Task<Result<ToolExecutionResultDto>> ApproveToolExecutionAsync(Guid executionId, CancellationToken cancellationToken = default);
    Task<Result> RejectToolExecutionAsync(Guid executionId, string? reason, CancellationToken cancellationToken = default);
    Task<Result<PaginatedList<ToolExecutionDto>>> GetToolExecutionHistoryAsync(Guid? sessionId, int page, int pageSize, CancellationToken cancellationToken = default);
}
