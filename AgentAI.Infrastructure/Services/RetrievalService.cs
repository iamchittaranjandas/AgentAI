using AgentAI.Application.Common;
using AgentAI.Application.DTOs.Retrieval;
using AgentAI.Application.Interfaces.Repositories;
using AgentAI.Application.Interfaces.Services;
using System.Diagnostics;

namespace AgentAI.Infrastructure.Services;

public class RetrievalService : IRetrievalService
{
    private readonly IRetrievalChunkRepository _chunkRepository;
    private readonly IEmbeddingService _embeddingService;

    public RetrievalService(
        IRetrievalChunkRepository chunkRepository,
        IEmbeddingService embeddingService)
    {
        _chunkRepository = chunkRepository;
        _embeddingService = embeddingService;
    }

    public async Task<Result<RetrievalResultDto>> SearchAsync(RetrievalRequest request, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();

        var queryEmbedding = await _embeddingService.GenerateEmbeddingAsync(request.Query, cancellationToken);

        var chunks = await _chunkRepository.SearchByEmbeddingAsync(
            queryEmbedding,
            request.RepositoryPath,
            request.MaxResults,
            request.MinSimilarity,
            cancellationToken);

        stopwatch.Stop();

        var chunkDtos = chunks.Select(c => new RetrievalChunkDto
        {
            Id = c.Id,
            FilePath = c.FilePath,
            FileName = c.FileName,
            ChunkContent = c.ChunkContent,
            StartLine = c.StartLine,
            EndLine = c.EndLine,
            ChunkType = c.ChunkType,
            ClassName = c.ClassName,
            MethodName = c.MethodName,
            SimilarityScore = 0.0f
        }).ToList();

        var result = new RetrievalResultDto
        {
            Chunks = chunkDtos,
            TotalResults = chunks.Count,
            SearchDuration = stopwatch.Elapsed
        };

        return Result<RetrievalResultDto>.SuccessResult(result);
    }

    public async Task<Result<List<RetrievalChunkDto>>> GetRelevantChunksAsync(string query, string? repositoryPath, int maxResults, CancellationToken cancellationToken = default)
    {
        var queryEmbedding = await _embeddingService.GenerateEmbeddingAsync(query, cancellationToken);

        var chunks = await _chunkRepository.SearchByEmbeddingAsync(
            queryEmbedding,
            repositoryPath,
            maxResults,
            0.7f,
            cancellationToken);

        var chunkDtos = chunks.Select(c => new RetrievalChunkDto
        {
            Id = c.Id,
            FilePath = c.FilePath,
            FileName = c.FileName,
            ChunkContent = c.ChunkContent,
            StartLine = c.StartLine,
            EndLine = c.EndLine,
            ChunkType = c.ChunkType,
            ClassName = c.ClassName,
            MethodName = c.MethodName,
            SimilarityScore = 0.0f
        }).ToList();

        return Result<List<RetrievalChunkDto>>.SuccessResult(chunkDtos);
    }
}
