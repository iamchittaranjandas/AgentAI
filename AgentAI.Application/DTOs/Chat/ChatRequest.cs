using System.ComponentModel.DataAnnotations;

namespace AgentAI.Application.DTOs.Chat;

public class ChatRequest
{
    [Required]
    [MaxLength(10000)]
    public string Prompt { get; set; } = string.Empty;

    [Required]
    public Guid SessionId { get; set; }

    public bool IncludeContext { get; set; } = true;

    public int MaxContextChunks { get; set; } = 5;

    public string? RepositoryPath { get; set; }

    public string? Branch { get; set; }
}
