using AgentAI.Domain.Entities;

namespace AgentAI.Application.Interfaces.Repositories;

public interface IPromptRecordRepository
{
    Task<PromptRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<PromptRecord>> GetBySessionIdAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task<PromptRecord> AddAsync(PromptRecord promptRecord, CancellationToken cancellationToken = default);
    Task UpdateAsync(PromptRecord promptRecord, CancellationToken cancellationToken = default);
}
