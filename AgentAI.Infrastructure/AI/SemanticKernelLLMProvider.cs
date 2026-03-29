using AgentAI.Application.Interfaces.Infrastructure;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AgentAI.Infrastructure.AI;

public class SemanticKernelLLMProvider : ILLMProvider
{
    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatCompletion;

    public SemanticKernelLLMProvider(Kernel kernel, IChatCompletionService chatCompletion)
    {
        _kernel = kernel;
        _chatCompletion = chatCompletion;
    }

    public async Task<string> GenerateResponseAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var chatHistory = new ChatHistory();
        chatHistory.AddSystemMessage("You are an AI coding assistant helping developers with their code.");
        chatHistory.AddUserMessage(prompt);

        var response = await _chatCompletion.GetChatMessageContentAsync(chatHistory, cancellationToken: cancellationToken);
        return response.Content ?? string.Empty;
    }

    public async Task<string> GenerateResponseWithContextAsync(string prompt, List<string> contextChunks, CancellationToken cancellationToken = default)
    {
        var chatHistory = new ChatHistory();
        chatHistory.AddSystemMessage("You are an AI coding assistant. Use the provided code context to answer questions accurately.");

        var contextMessage = "Here is the relevant code context:\n\n" + string.Join("\n\n---\n\n", contextChunks);
        chatHistory.AddSystemMessage(contextMessage);
        chatHistory.AddUserMessage(prompt);

        var response = await _chatCompletion.GetChatMessageContentAsync(chatHistory, cancellationToken: cancellationToken);
        return response.Content ?? string.Empty;
    }

    public async IAsyncEnumerable<string> StreamResponseAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var chatHistory = new ChatHistory();
        chatHistory.AddSystemMessage("You are an AI coding assistant helping developers with their code.");
        chatHistory.AddUserMessage(prompt);

        await foreach (var chunk in _chatCompletion.GetStreamingChatMessageContentsAsync(chatHistory, cancellationToken: cancellationToken))
        {
            if (!string.IsNullOrEmpty(chunk.Content))
            {
                yield return chunk.Content;
            }
        }
    }
}
