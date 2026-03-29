using AgentAI.Application.Common;
using AgentAI.Application.DTOs.Admin;
using AgentAI.Application.DTOs.Indexing;
using AgentAI.Application.Interfaces.Repositories;
using AgentAI.Application.Interfaces.Services;
using Mapster;

namespace AgentAI.Infrastructure.Services;

public class IndexingService : IIndexingService
{
    private readonly IRepositoryRepository _repositoryRepository;

    public IndexingService(IRepositoryRepository repositoryRepository)
    {
        _repositoryRepository = repositoryRepository;
    }

    public async Task<Result<IndexingStatusDto>> StartIndexingAsync(IndexingRequest request, CancellationToken cancellationToken = default)
    {
        var jobId = Guid.NewGuid();

        var status = new IndexingStatusDto
        {
            JobId = jobId,
            Status = "Started",
            TotalFiles = 0,
            ProcessedFiles = 0,
            TotalChunks = 0,
            StartedAt = DateTime.UtcNow
        };

        return Result<IndexingStatusDto>.SuccessResult(status, "Indexing job started (implementation pending)");
    }

    public async Task<Result<IndexingStatusDto>> GetIndexingStatusAsync(Guid jobId, CancellationToken cancellationToken = default)
    {
        var status = new IndexingStatusDto
        {
            JobId = jobId,
            Status = "Completed",
            TotalFiles = 0,
            ProcessedFiles = 0,
            TotalChunks = 0,
            StartedAt = DateTime.UtcNow.AddMinutes(-5),
            CompletedAt = DateTime.UtcNow
        };

        return Result<IndexingStatusDto>.SuccessResult(status);
    }

    public async Task<Result<List<RepositoryDto>>> GetIndexedRepositoriesAsync(CancellationToken cancellationToken = default)
    {
        var repositories = await _repositoryRepository.GetAllActiveAsync(cancellationToken);
        var dtos = repositories.Adapt<List<RepositoryDto>>();

        return Result<List<RepositoryDto>>.SuccessResult(dtos);
    }
}
