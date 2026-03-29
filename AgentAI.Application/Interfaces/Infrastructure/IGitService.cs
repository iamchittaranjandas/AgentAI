namespace AgentAI.Application.Interfaces.Infrastructure;

public interface IGitService
{
    Task<string> GetCurrentBranchAsync(string repositoryPath, CancellationToken cancellationToken = default);
    Task<List<string>> GetModifiedFilesAsync(string repositoryPath, CancellationToken cancellationToken = default);
    Task<string> GetDiffAsync(string repositoryPath, string? filePath, CancellationToken cancellationToken = default);
    Task<bool> IsGitRepositoryAsync(string path, CancellationToken cancellationToken = default);
}
