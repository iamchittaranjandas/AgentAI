using AgentAI.Domain.Entities;
using AgentAI.Domain.Interfaces;
using AgentAI.Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AgentAI.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly IMediator? _mediator;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<PromptRecord> PromptRecords { get; set; }
    public DbSet<RetrievalChunk> RetrievalChunks { get; set; }
    public DbSet<ToolExecution> ToolExecutions { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<MemoryItem> MemoryItems { get; set; }
    public DbSet<CodeFile> CodeFiles { get; set; }
    public DbSet<Repository> Repositories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Seed the default system user used by the application
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Email = "admin@agentai.local",
            FullName = "System Administrator",
            PasswordHash = "not-used",
            Role = Domain.Enums.UserRole.Admin,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        
        if (_mediator != null)
        {
            await DomainEventDispatcher.DispatchDomainEventsAsync(this, _mediator, cancellationToken);
        }
        
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Domain.Interfaces.IAuditableEntity &&
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (Domain.Interfaces.IAuditableEntity)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
