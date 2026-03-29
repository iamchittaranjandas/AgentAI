using AgentAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgentAI.Infrastructure.Persistence.Configurations;

public class RepositoryConfiguration : IEntityTypeConfiguration<Repository>
{
    public void Configure(EntityTypeBuilder<Repository> builder)
    {
        builder.ToTable("Repositories");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(r => r.Path)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(r => r.GitUrl)
            .HasMaxLength(1000);

        builder.Property(r => r.DefaultBranch)
            .HasMaxLength(200)
            .HasDefaultValue("main");

        builder.Property(r => r.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(r => r.TotalFiles)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(r => r.IndexedFiles)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.HasIndex(r => r.Path)
            .IsUnique();

        builder.HasIndex(r => r.Name);
        builder.HasIndex(r => r.IsActive);
        builder.HasIndex(r => r.LastIndexed);
    }
}
