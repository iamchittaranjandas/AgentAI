namespace AgentAI.Application.DTOs.Retrieval;

public class RetrievalChunkDto
{
    public Guid Id { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string ChunkContent { get; set; } = string.Empty;
    public int StartLine { get; set; }
    public int EndLine { get; set; }
    public string ChunkType { get; set; } = string.Empty;
    public string? ClassName { get; set; }
    public string? MethodName { get; set; }
    public float SimilarityScore { get; set; }
}
