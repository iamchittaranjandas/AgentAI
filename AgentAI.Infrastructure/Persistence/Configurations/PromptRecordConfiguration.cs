using AgentAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgentAI.Infrastructure.Persistence.Configurations;

public class PromptRecordConfiguration : IEntityTypeConfiguration<PromptRecord>
{
    public void Configure(EntityTypeBuilder<PromptRecord> builder)
    {
        builder.ToTable("PromptRecords");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.SessionId)
            .IsRequired();

        builder.Property(p => p.UserPrompt)
            .IsRequired()
            .HasMaxLength(10000);

        builder.Property(p => p.DetectedIntent)
            .IsRequired();

        builder.Property(p => p.AssistantResponse)
            .HasMaxLength(50000);

        builder.Property(p => p.TokensUsed)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.ContextChunksRetrieved)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.ResponseTime)
            .IsRequired();

        builder.Property(p => p.WasSuccessful)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(p => p.ErrorMessage)
            .HasMaxLength(2000);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.HasIndex(p => p.SessionId);
        builder.HasIndex(p => p.DetectedIntent);
        builder.HasIndex(p => p.CreatedAt);
        builder.HasIndex(p => p.WasSuccessful);
    }
}
