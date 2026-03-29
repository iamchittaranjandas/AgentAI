using AgentAI.Application.Interfaces.Repositories;
using AgentAI.Domain.Entities;
using AgentAI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgentAI.Infrastructure.Repositories;

public class MemoryItemRepository : IMemoryItemRepository
{
    private readonly ApplicationDbContext _context;

    public MemoryItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MemoryItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.MemoryItems
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<List<MemoryItem>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.MemoryItems
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.Priority)
            .ThenByDescending(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<MemoryItem>> GetByRepositoryPathAsync(string repositoryPath, CancellationToken cancellationToken = default)
    {
        return await _context.MemoryItems
            .Where(m => m.RepositoryPath == repositoryPath)
            .OrderByDescending(m => m.Priority)
            .ThenByDescending(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<MemoryItem> AddAsync(MemoryItem memoryItem, CancellationToken cancellationToken = default)
    {
        await _context.MemoryItems.AddAsync(memoryItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return memoryItem;
    }

    public async Task UpdateAsync(MemoryItem memoryItem, CancellationToken cancellationToken = default)
    {
        _context.MemoryItems.Update(memoryItem);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(MemoryItem memoryItem, CancellationToken cancellationToken = default)
    {
        _context.MemoryItems.Remove(memoryItem);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
