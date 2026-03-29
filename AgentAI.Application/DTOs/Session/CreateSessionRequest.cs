using System.ComponentModel.DataAnnotations;

namespace AgentAI.Application.DTOs.Session;

public class CreateSessionRequest
{
    [MaxLength(500)]
    public string? Title { get; set; }

    [MaxLength(1000)]
    public string? RepositoryPath { get; set; }

    [MaxLength(200)]
    public string? Branch { get; set; }
}
