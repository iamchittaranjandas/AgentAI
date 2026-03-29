using AgentAI.Domain.Entities;

namespace AgentAI.Application.Interfaces.Repositories;

public interface IRetrievalChunkRepository
{
    Task<RetrievalChunk?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<RetrievalChunk>> SearchByEmbeddingAsync(float[] embedding, string? repositoryPath, int maxResults, float minSimilarity, CancellationToken cancellationToken = default);
    Task<RetrievalChunk> AddAsync(RetrievalChunk chunk, CancellationToken cancellationToken = default);
    Task AddRangeAsync(List<RetrievalChunk> chunks, CancellationToken cancellationToken = default);
    Task DeleteByRepositoryPathAsync(string repositoryPath, CancellationToken cancellationToken = default);
}
