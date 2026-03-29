namespace AgentAI.Application.Interfaces.Infrastructure;

public interface IVectorStore
{
    Task<List<(Guid Id, float Similarity)>> SearchAsync(float[] embedding, int maxResults, float minSimilarity, CancellationToken cancellationToken = default);
    Task AddAsync(Guid id, float[] embedding, CancellationToken cancellationToken = default);
    Task AddRangeAsync(Dictionary<Guid, float[]> embeddings, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
