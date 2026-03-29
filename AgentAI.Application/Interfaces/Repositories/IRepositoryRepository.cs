using AgentAI.Domain.Entities;

namespace AgentAI.Application.Interfaces.Repositories;

public interface IRepositoryRepository
{
    Task<Repository?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Repository?> GetByPathAsync(string path, CancellationToken cancellationToken = default);
    Task<List<Repository>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<Repository> AddAsync(Repository repository, CancellationToken cancellationToken = default);
    Task UpdateAsync(Repository repository, CancellationToken cancellationToken = default);
}
