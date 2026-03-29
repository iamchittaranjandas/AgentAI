using AgentAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgentAI.Infrastructure.Persistence.Configurations;

public class RetrievalChunkConfiguration : IEntityTypeConfiguration<RetrievalChunk>
{
    public void Configure(EntityTypeBuilder<RetrievalChunk> builder)
    {
        builder.ToTable("RetrievalChunks");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.FilePath)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(r => r.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(r => r.FileExtension)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.ChunkContent)
            .IsRequired()
            .HasMaxLength(20000);

        builder.Property(r => r.StartLine)
            .IsRequired();

        builder.Property(r => r.EndLine)
            .IsRequired();

        builder.Property(r => r.ChunkType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.ClassName)
            .HasMaxLength(500);

        builder.Property(r => r.MethodName)
            .HasMaxLength(500);

        builder.Property(r => r.Embedding)
            .IsRequired();

        builder.Property(r => r.RepositoryPath)
            .HasMaxLength(1000);

        builder.Property(r => r.Branch)
            .HasMaxLength(200);

        builder.Property(r => r.IndexedAt)
            .IsRequired();

        builder.Property(r => r.FileLastModified)
            .IsRequired();

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.HasIndex(r => r.FilePath);
        builder.HasIndex(r => r.FileName);
        builder.HasIndex(r => r.ChunkType);
        builder.HasIndex(r => r.IndexedAt);

        builder.HasOne(r => r.CodeFile)
            .WithMany(c => c.Chunks)
            .HasForeignKey(r => r.CodeFileId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
