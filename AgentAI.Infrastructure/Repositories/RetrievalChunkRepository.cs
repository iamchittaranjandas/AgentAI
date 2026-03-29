using AgentAI.Application.Interfaces.Repositories;
using AgentAI.Domain.Entities;
using AgentAI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgentAI.Infrastructure.Repositories;

public class RetrievalChunkRepository : IRetrievalChunkRepository
{
    private readonly ApplicationDbContext _context;

    public RetrievalChunkRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RetrievalChunk?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.RetrievalChunks
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<List<RetrievalChunk>> SearchByEmbeddingAsync(float[] embedding, string? repositoryPath, int maxResults, float minSimilarity, CancellationToken cancellationToken = default)
    {
        var query = _context.RetrievalChunks.AsQueryable();

        if (!string.IsNullOrEmpty(repositoryPath))
        {
            query = query.Where(r => r.RepositoryPath == repositoryPath);
        }

        var chunks = await query.ToListAsync(cancellationToken);

        var results = chunks
            .Select(chunk => new
            {
                Chunk = chunk,
                Similarity = CosineSimilarity(embedding, chunk.Embedding)
            })
            .Where(x => x.Similarity >= minSimilarity)
            .OrderByDescending(x => x.Similarity)
            .Take(maxResults)
            .Select(x => x.Chunk)
            .ToList();

        return results;
    }

    public async Task<RetrievalChunk> AddAsync(RetrievalChunk chunk, CancellationToken cancellationToken = default)
    {
        await _context.RetrievalChunks.AddAsync(chunk, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return chunk;
    }

    public async Task AddRangeAsync(List<RetrievalChunk> chunks, CancellationToken cancellationToken = default)
    {
        await _context.RetrievalChunks.AddRangeAsync(chunks, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteByRepositoryPathAsync(string repositoryPath, CancellationToken cancellationToken = default)
    {
        var chunks = await _context.RetrievalChunks
            .Where(r => r.RepositoryPath == repositoryPath)
            .ToListAsync(cancellationToken);

        _context.RetrievalChunks.RemoveRange(chunks);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static float CosineSimilarity(float[] a, float[] b)
    {
        if (a.Length != b.Length)
            return 0;

        float dotProduct = 0;
        float magnitudeA = 0;
        float magnitudeB = 0;

        for (int i = 0; i < a.Length; i++)
        {
            dotProduct += a[i] * b[i];
            magnitudeA += a[i] * a[i];
            magnitudeB += b[i] * b[i];
        }

        if (magnitudeA == 0 || magnitudeB == 0)
            return 0;

        return dotProduct / (MathF.Sqrt(magnitudeA) * MathF.Sqrt(magnitudeB));
    }
}
