using AgentAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgentAI.Infrastructure.Persistence.Configurations;

public class CodeFileConfiguration : IEntityTypeConfiguration<CodeFile>
{
    public void Configure(EntityTypeBuilder<CodeFile> builder)
    {
        builder.ToTable("CodeFiles");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.RepositoryId)
            .IsRequired();

        builder.Property(c => c.FilePath)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(c => c.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.FileExtension)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.FileSize)
            .IsRequired();

        builder.Property(c => c.FileHash)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(c => c.LineCount)
            .IsRequired();

        builder.Property(c => c.LastModified)
            .IsRequired();

        builder.Property(c => c.LastIndexed)
            .IsRequired();

        builder.Property(c => c.IsIndexed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(c => c.ChunkCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.HasIndex(c => c.RepositoryId);
        builder.HasIndex(c => c.FilePath);
        builder.HasIndex(c => c.FileHash);
        builder.HasIndex(c => c.LastIndexed);
        builder.HasIndex(c => c.IsIndexed);

        builder.HasIndex(c => new { c.RepositoryId, c.FilePath })
            .IsUnique();

        builder.HasOne(c => c.Repository)
            .WithMany(r => r.CodeFiles)
            .HasForeignKey(c => c.RepositoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
