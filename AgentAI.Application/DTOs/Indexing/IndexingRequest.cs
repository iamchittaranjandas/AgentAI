using System.ComponentModel.DataAnnotations;

namespace AgentAI.Application.DTOs.Indexing;

public class IndexingRequest
{
    [Required]
    [MaxLength(1000)]
    public string RepositoryPath { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Branch { get; set; }

    public bool FullReindex { get; set; }

    public List<string>? FileExtensions { get; set; }

    public List<string>? ExcludePatterns { get; set; }
}
