namespace AgentAI.Application.DTOs.Retrieval;

public class RetrievalResultDto
{
    public List<RetrievalChunkDto> Chunks { get; set; } = new();
    public int TotalResults { get; set; }
    public TimeSpan SearchDuration { get; set; }
}
