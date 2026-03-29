using AgentAI.Application.Interfaces.Repositories;
using AgentAI.Domain.Entities;
using AgentAI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgentAI.Infrastructure.Repositories;

public class CodeFileRepository : ICodeFileRepository
{
    private readonly ApplicationDbContext _context;

    public CodeFileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CodeFile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.CodeFiles
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<CodeFile?> GetByRepositoryAndPathAsync(Guid repositoryId, string filePath, CancellationToken cancellationToken = default)
    {
        return await _context.CodeFiles
            .FirstOrDefaultAsync(c => c.RepositoryId == repositoryId && c.FilePath == filePath, cancellationToken);
    }

    public async Task<List<CodeFile>> GetByRepositoryIdAsync(Guid repositoryId, CancellationToken cancellationToken = default)
    {
        return await _context.CodeFiles
            .Where(c => c.RepositoryId == repositoryId)
            .OrderBy(c => c.FilePath)
            .ToListAsync(cancellationToken);
    }

    public async Task<CodeFile> AddAsync(CodeFile codeFile, CancellationToken cancellationToken = default)
    {
        await _context.CodeFiles.AddAsync(codeFile, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return codeFile;
    }

    public async Task UpdateAsync(CodeFile codeFile, CancellationToken cancellationToken = default)
    {
        _context.CodeFiles.Update(codeFile);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
