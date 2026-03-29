using AgentAI.Domain.Interfaces;

namespace AgentAI.Domain.Entities;

public class CodeFile : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid RepositoryId { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string FileHash { get; set; } = string.Empty;
    public int LineCount { get; set; }
    public DateTime LastModified { get; set; }
    public DateTime LastIndexed { get; set; }
    public bool IsIndexed { get; set; }
    public int ChunkCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public Repository Repository { get; set; } = null!;
    public ICollection<RetrievalChunk> Chunks { get; set; } = new List<RetrievalChunk>();
}
