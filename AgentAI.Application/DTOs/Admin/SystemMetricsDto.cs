namespace AgentAI.Application.DTOs.Admin;

public class SystemMetricsDto
{
    public int TotalUsers { get; set; }
    public int ActiveSessions { get; set; }
    public int TotalPrompts { get; set; }
    public int TotalToolExecutions { get; set; }
    public int IndexedRepositories { get; set; }
    public int TotalChunks { get; set; }
    public Dictionary<string, int> PromptsByIntent { get; set; } = new();
    public Dictionary<string, int> ToolExecutionsByType { get; set; } = new();
}
