namespace AgentAI.Application.DTOs.Admin;

public class RepositoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string? GitUrl { get; set; }
    public DateTime? LastIndexed { get; set; }
    public bool IsActive { get; set; }
    public int TotalFiles { get; set; }
    public int IndexedFiles { get; set; }
}
