using AgentAI.Application.Common;
using AgentAI.Application.Interfaces.Repositories;
using AgentAI.Domain.Entities;
using AgentAI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgentAI.Infrastructure.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly ApplicationDbContext _context;

    public SessionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Session?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sessions
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Session?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sessions
            .Include(s => s.PromptRecords)
            .Include(s => s.ToolExecutions)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<PaginatedList<Session>> GetByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Sessions
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.LastActivityAt);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<Session>(items, totalCount, page, pageSize);
    }

    public async Task<Session> AddAsync(Session session, CancellationToken cancellationToken = default)
    {
        await _context.Sessions.AddAsync(session, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return session;
    }

    public async Task UpdateAsync(Session session, CancellationToken cancellationToken = default)
    {
        _context.Sessions.Update(session);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Session session, CancellationToken cancellationToken = default)
    {
        _context.Sessions.Remove(session);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
