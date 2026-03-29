using AgentAI.Domain.Entities;

namespace AgentAI.Application.Interfaces.Repositories;

public interface ICodeFileRepository
{
    Task<CodeFile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CodeFile?> GetByRepositoryAndPathAsync(Guid repositoryId, string filePath, CancellationToken cancellationToken = default);
    Task<List<CodeFile>> GetByRepositoryIdAsync(Guid repositoryId, CancellationToken cancellationToken = default);
    Task<CodeFile> AddAsync(CodeFile codeFile, CancellationToken cancellationToken = default);
    Task UpdateAsync(CodeFile codeFile, CancellationToken cancellationToken = default);
}
