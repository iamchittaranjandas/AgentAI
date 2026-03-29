namespace AgentAI.Application.Interfaces.Infrastructure;

public interface ILLMProvider
{
    Task<string> GenerateResponseAsync(string prompt, CancellationToken cancellationToken = default);
    Task<string> GenerateResponseWithContextAsync(string prompt, List<string> contextChunks, CancellationToken cancellationToken = default);
    IAsyncEnumerable<string> StreamResponseAsync(string prompt, CancellationToken cancellationToken = default);
}
