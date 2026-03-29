using AgentAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgentAI.Infrastructure.Persistence.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("Sessions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.UserId)
            .IsRequired();

        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(s => s.Status)
            .IsRequired();

        builder.Property(s => s.RepositoryPath)
            .HasMaxLength(1000);

        builder.Property(s => s.Branch)
            .HasMaxLength(200);

        builder.Property(s => s.StartedAt)
            .IsRequired();

        builder.Property(s => s.LastActivityAt)
            .IsRequired();

        builder.Property(s => s.MessageCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.HasIndex(s => s.UserId);
        builder.HasIndex(s => s.Status);
        builder.HasIndex(s => s.LastActivityAt);
        builder.HasIndex(s => s.CreatedAt);

        builder.HasMany(s => s.PromptRecords)
            .WithOne(p => p.Session)
            .HasForeignKey(p => p.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.ToolExecutions)
            .WithOne(t => t.Session)
            .HasForeignKey(t => t.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
