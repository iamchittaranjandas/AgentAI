using AgentAI.Application.Common;
using AgentAI.Domain.Entities;

namespace AgentAI.Application.Interfaces.Repositories;

public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Session?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PaginatedList<Session>> GetByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Session> AddAsync(Session session, CancellationToken cancellationToken = default);
    Task UpdateAsync(Session session, CancellationToken cancellationToken = default);
    Task DeleteAsync(Session session, CancellationToken cancellationToken = default);
}
