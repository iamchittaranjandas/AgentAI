using AgentAI.Application.Common;
using AgentAI.Application.DTOs.Retrieval;

namespace AgentAI.Application.Interfaces.Services;

public interface IRetrievalService
{
    Task<Result<RetrievalResultDto>> SearchAsync(RetrievalRequest request, CancellationToken cancellationToken = default);
    Task<Result<List<RetrievalChunkDto>>> GetRelevantChunksAsync(string query, string? repositoryPath, int maxResults, CancellationToken cancellationToken = default);
}
