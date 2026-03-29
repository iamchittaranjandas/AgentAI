using AgentAI.Application.Interfaces.Repositories;
using AgentAI.Domain.Entities;
using AgentAI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgentAI.Infrastructure.Repositories;

public class PromptRecordRepository : IPromptRecordRepository
{
    private readonly ApplicationDbContext _context;

    public PromptRecordRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PromptRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.PromptRecords
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<List<PromptRecord>> GetBySessionIdAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        return await _context.PromptRecords
            .Where(p => p.SessionId == sessionId)
            .OrderBy(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<PromptRecord> AddAsync(PromptRecord promptRecord, CancellationToken cancellationToken = default)
    {
        await _context.PromptRecords.AddAsync(promptRecord, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return promptRecord;
    }

    public async Task UpdateAsync(PromptRecord promptRecord, CancellationToken cancellationToken = default)
    {
        _context.PromptRecords.Update(promptRecord);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
