using AgentAI.Application.Common;
using AgentAI.Domain.Entities;

namespace AgentAI.Application.Interfaces.Repositories;

public interface IAuditLogRepository
{
    Task<PaginatedList<AuditLog>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PaginatedList<AuditLog>> GetByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<AuditLog> AddAsync(AuditLog auditLog, CancellationToken cancellationToken = default);
}
