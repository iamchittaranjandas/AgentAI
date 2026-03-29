using AgentAI.Application.Common;
using AgentAI.Domain.Entities;

namespace AgentAI.Application.Interfaces.Repositories;

public interface IToolExecutionRepository
{
    Task<ToolExecution?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PaginatedList<ToolExecution>> GetBySessionIdAsync(Guid sessionId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<ToolExecution> AddAsync(ToolExecution toolExecution, CancellationToken cancellationToken = default);
    Task UpdateAsync(ToolExecution toolExecution, CancellationToken cancellationToken = default);
}
