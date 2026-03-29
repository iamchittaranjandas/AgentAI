using AgentAI.Application.Interfaces.Repositories;
using AgentAI.Domain.Entities;
using AgentAI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgentAI.Infrastructure.Repositories;

public class RepositoryRepository : IRepositoryRepository
{
    private readonly ApplicationDbContext _context;

    public RepositoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Repository?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Repositories
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Repository?> GetByPathAsync(string path, CancellationToken cancellationToken = default)
    {
        return await _context.Repositories
            .FirstOrDefaultAsync(r => r.Path == path, cancellationToken);
    }

    public async Task<List<Repository>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Repositories
            .Where(r => r.IsActive)
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Repository> AddAsync(Repository repository, CancellationToken cancellationToken = default)
    {
        await _context.Repositories.AddAsync(repository, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return repository;
    }

    public async Task UpdateAsync(Repository repository, CancellationToken cancellationToken = default)
    {
        _context.Repositories.Update(repository);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
