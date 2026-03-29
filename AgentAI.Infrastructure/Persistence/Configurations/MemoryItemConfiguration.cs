using AgentAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgentAI.Infrastructure.Persistence.Configurations;

public class MemoryItemConfiguration : IEntityTypeConfiguration<MemoryItem>
{
    public void Configure(EntityTypeBuilder<MemoryItem> builder)
    {
        builder.ToTable("MemoryItems");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.RepositoryPath)
            .HasMaxLength(1000);

        builder.Property(m => m.Key)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(m => m.Value)
            .IsRequired()
            .HasMaxLength(20000);

        builder.Property(m => m.Category)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.Priority)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.HasIndex(m => m.UserId);
        builder.HasIndex(m => m.RepositoryPath);
        builder.HasIndex(m => m.Category);
        builder.HasIndex(m => m.Priority);
        builder.HasIndex(m => m.CreatedAt);

        builder.HasIndex(m => new { m.UserId, m.RepositoryPath, m.Key })
            .IsUnique();
    }
}
