using System.ComponentModel.DataAnnotations;

namespace AgentAI.Application.DTOs.Retrieval;

public class RetrievalRequest
{
    [Required]
    [MaxLength(5000)]
    public string Query { get; set; } = string.Empty;

    public string? RepositoryPath { get; set; }

    public string? Branch { get; set; }

    public List<string>? FileExtensions { get; set; }

    public string? ChunkType { get; set; }

    public int MaxResults { get; set; } = 10;

    public float MinSimilarity { get; set; } = 0.7f;
}
