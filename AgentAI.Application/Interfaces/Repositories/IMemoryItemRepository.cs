using AgentAI.Domain.Entities;

namespace AgentAI.Application.Interfaces.Repositories;

public interface IMemoryItemRepository
{
    Task<MemoryItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<MemoryItem>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<List<MemoryItem>> GetByRepositoryPathAsync(string repositoryPath, CancellationToken cancellationToken = default);
    Task<MemoryItem> AddAsync(MemoryItem memoryItem, CancellationToken cancellationToken = default);
    Task UpdateAsync(MemoryItem memoryItem, CancellationToken cancellationToken = default);
    Task DeleteAsync(MemoryItem memoryItem, CancellationToken cancellationToken = default);
}
