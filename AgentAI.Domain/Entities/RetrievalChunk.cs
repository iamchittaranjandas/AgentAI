using AgentAI.Domain.Interfaces;

namespace AgentAI.Domain.Entities;

public class RetrievalChunk : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid? CodeFileId { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public string ChunkContent { get; set; } = string.Empty;
    public int StartLine { get; set; }
    public int EndLine { get; set; }
    public string ChunkType { get; set; } = string.Empty;
    public string? ClassName { get; set; }
    public string? MethodName { get; set; }
    public float[] Embedding { get; set; } = Array.Empty<float>();
    public string? RepositoryPath { get; set; }
    public string? Branch { get; set; }
    public DateTime IndexedAt { get; set; }
    public DateTime FileLastModified { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public CodeFile? CodeFile { get; set; }
}
