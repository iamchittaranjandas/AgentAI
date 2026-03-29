namespace AgentAI.Application.Interfaces.Infrastructure;

public interface IFileSystemService
{
    Task<string> ReadFileAsync(string filePath, CancellationToken cancellationToken = default);
    Task WriteFileAsync(string filePath, string content, CancellationToken cancellationToken = default);
    Task<bool> FileExistsAsync(string filePath, CancellationToken cancellationToken = default);
    Task<List<string>> GetFilesAsync(string directoryPath, string searchPattern, CancellationToken cancellationToken = default);
    Task<List<string>> ParseCodeFileAsync(string filePath, CancellationToken cancellationToken = default);
}
