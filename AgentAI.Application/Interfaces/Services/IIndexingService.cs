using AgentAI.Application.Common;
using AgentAI.Application.DTOs.Admin;
using AgentAI.Application.DTOs.Indexing;

namespace AgentAI.Application.Interfaces.Services;

public interface IIndexingService
{
    Task<Result<IndexingStatusDto>> StartIndexingAsync(IndexingRequest request, CancellationToken cancellationToken = default);
    Task<Result<IndexingStatusDto>> GetIndexingStatusAsync(Guid jobId, CancellationToken cancellationToken = default);
    Task<Result<List<RepositoryDto>>> GetIndexedRepositoriesAsync(CancellationToken cancellationToken = default);
}
