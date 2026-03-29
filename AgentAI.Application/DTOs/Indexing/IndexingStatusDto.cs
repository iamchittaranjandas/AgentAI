namespace AgentAI.Application.DTOs.Indexing;

public class IndexingStatusDto
{
    public Guid JobId { get; set; }
    public string Status { get; set; } = string.Empty;
    public int TotalFiles { get; set; }
    public int ProcessedFiles { get; set; }
    public int TotalChunks { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }
}
