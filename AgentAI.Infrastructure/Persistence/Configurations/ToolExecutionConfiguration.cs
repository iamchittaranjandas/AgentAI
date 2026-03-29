using AgentAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgentAI.Infrastructure.Persistence.Configurations;

public class ToolExecutionConfiguration : IEntityTypeConfiguration<ToolExecution>
{
    public void Configure(EntityTypeBuilder<ToolExecution> builder)
    {
        builder.ToTable("ToolExecutions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.SessionId)
            .IsRequired();

        builder.Property(t => t.ToolType)
            .IsRequired();

        builder.Property(t => t.ToolName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Action)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.InputParameters)
            .IsRequired()
            .HasMaxLength(10000);

        builder.Property(t => t.Output)
            .HasMaxLength(50000);

        builder.Property(t => t.ApprovalStatus)
            .IsRequired();

        builder.Property(t => t.RiskLevel)
            .IsRequired();

        builder.Property(t => t.RequestedAt)
            .IsRequired();

        builder.Property(t => t.WasSuccessful)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(t => t.ErrorMessage)
            .HasMaxLength(2000);

        builder.Property(t => t.ApprovedBy)
            .HasMaxLength(256);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.HasIndex(t => t.SessionId);
        builder.HasIndex(t => t.ToolType);
        builder.HasIndex(t => t.ApprovalStatus);
        builder.HasIndex(t => t.RiskLevel);
        builder.HasIndex(t => t.RequestedAt);
        builder.HasIndex(t => t.WasSuccessful);

        builder.HasOne(t => t.PromptRecord)
            .WithMany()
            .HasForeignKey(t => t.PromptRecordId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
