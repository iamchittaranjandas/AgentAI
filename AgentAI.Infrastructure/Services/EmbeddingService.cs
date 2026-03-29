using AgentAI.Application.Interfaces.Services;
using Microsoft.SemanticKernel.Embeddings;

namespace AgentAI.Infrastructure.Services;

public class EmbeddingService : IEmbeddingService
{
    private readonly ITextEmbeddingGenerationService _embeddingGenerator;

    public EmbeddingService(ITextEmbeddingGenerationService embeddingGenerator)
    {
        _embeddingGenerator = embeddingGenerator;
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
    {
        var embedding = await _embeddingGenerator.GenerateEmbeddingAsync(text, cancellationToken: cancellationToken);
        return embedding.ToArray();
    }

    public async Task<List<float[]>> GenerateEmbeddingsAsync(List<string> texts, CancellationToken cancellationToken = default)
    {
        var embeddings = await _embeddingGenerator.GenerateEmbeddingsAsync(texts, cancellationToken: cancellationToken);
        return embeddings.Select(e => e.ToArray()).ToList();
    }
}
