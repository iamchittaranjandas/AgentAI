using AgentAI.Application.Common;
using AgentAI.Application.Interfaces.Repositories;
using AgentAI.Domain.Entities;
using AgentAI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgentAI.Infrastructure.Repositories;

public class ToolExecutionRepository : IToolExecutionRepository
{
    private readonly ApplicationDbContext _context;

    public ToolExecutionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ToolExecution?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ToolExecutions
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<PaginatedList<ToolExecution>> GetBySessionIdAsync(Guid sessionId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.ToolExecutions
            .Where(t => t.SessionId == sessionId)
            .OrderByDescending(t => t.RequestedAt);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<ToolExecution>(items, totalCount, page, pageSize);
    }

    public async Task<ToolExecution> AddAsync(ToolExecution toolExecution, CancellationToken cancellationToken = default)
    {
        await _context.ToolExecutions.AddAsync(toolExecution, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return toolExecution;
    }

    public async Task UpdateAsync(ToolExecution toolExecution, CancellationToken cancellationToken = default)
    {
        _context.ToolExecutions.Update(toolExecution);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
