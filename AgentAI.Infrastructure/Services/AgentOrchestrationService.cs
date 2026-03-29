using AgentAI.Application.Common;
using AgentAI.Application.DTOs.Chat;
using AgentAI.Application.Interfaces.Infrastructure;
using AgentAI.Application.Interfaces.Repositories;
using AgentAI.Application.Interfaces.Services;
using AgentAI.Domain.Entities;
using AgentAI.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace AgentAI.Infrastructure.Services;

public class AgentOrchestrationService : IAgentOrchestrationService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IPromptRecordRepository _promptRecordRepository;
    private readonly IRetrievalService _retrievalService;
    private readonly ILLMProvider _llmProvider;
    private readonly ILogger<AgentOrchestrationService> _logger;

    public AgentOrchestrationService(
        ISessionRepository sessionRepository,
        IPromptRecordRepository promptRecordRepository,
        IRetrievalService retrievalService,
        ILLMProvider llmProvider,
        ILogger<AgentOrchestrationService> logger)
    {
        _sessionRepository = sessionRepository;
        _promptRecordRepository = promptRecordRepository;
        _retrievalService = retrievalService;
        _llmProvider = llmProvider;
        _logger = logger;
    }

    public async Task<Result<ChatResponse>> ProcessChatRequestAsync(ChatRequest request, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();

        var session = await _sessionRepository.GetByIdAsync(request.SessionId, cancellationToken);
        if (session == null)
        {
            return Result<ChatResponse>.FailureResult("Session not found");
        }

        var promptRecord = new PromptRecord
        {
            Id = Guid.NewGuid(),
            SessionId = request.SessionId,
            UserPrompt = request.Prompt,
            DetectedIntent = DetectIntent(request.Prompt),
            TokensUsed = 0,
            ContextChunksRetrieved = 0,
            ResponseTime = TimeSpan.Zero,
            WasSuccessful = false
        };

        try
        {
            List<string> contextChunks = new();
            if (request.IncludeContext)
            {
                var retrievalResult = await _retrievalService.GetRelevantChunksAsync(
                    request.Prompt,
                    request.RepositoryPath ?? session.RepositoryPath,
                    request.MaxContextChunks,
                    cancellationToken);

                if (retrievalResult.Success && retrievalResult.Data != null)
                {
                    contextChunks = retrievalResult.Data.Select(c => c.ChunkContent).ToList();
                    promptRecord.ContextChunksRetrieved = contextChunks.Count;
                }
            }

            string response;
            if (contextChunks.Any())
            {
                response = await _llmProvider.GenerateResponseWithContextAsync(request.Prompt, contextChunks, cancellationToken);
            }
            else
            {
                response = await _llmProvider.GenerateResponseAsync(request.Prompt, cancellationToken);
            }

            stopwatch.Stop();

            promptRecord.AssistantResponse = response;
            promptRecord.ResponseTime = stopwatch.Elapsed;
            promptRecord.TokensUsed = EstimateTokens(request.Prompt, response);
            promptRecord.WasSuccessful = true;

            await _promptRecordRepository.AddAsync(promptRecord, cancellationToken);

            session.MessageCount++;
            session.LastActivityAt = DateTime.UtcNow;
            await _sessionRepository.UpdateAsync(session, cancellationToken);

            var chatResponse = new ChatResponse
            {
                PromptRecordId = promptRecord.Id,
                Response = response,
                DetectedIntent = promptRecord.DetectedIntent,
                TokensUsed = promptRecord.TokensUsed,
                ContextChunksRetrieved = promptRecord.ContextChunksRetrieved,
                ResponseTime = promptRecord.ResponseTime
            };

            return Result<ChatResponse>.SuccessResult(chatResponse);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(ex,
                "Chat request failed for session {SessionId} | Intent: {Intent} | Elapsed: {ElapsedMs}ms | Error: {Message} | Inner: {InnerMessage}",
                request.SessionId,
                promptRecord.DetectedIntent,
                stopwatch.ElapsedMilliseconds,
                ex.Message,
                ex.InnerException?.Message ?? "none");

            promptRecord.ErrorMessage = GetErrorChain(ex);
            promptRecord.ResponseTime = stopwatch.Elapsed;
            await _promptRecordRepository.AddAsync(promptRecord, cancellationToken);

            return Result<ChatResponse>.FailureResult(GetAllErrors(ex));
        }
    }

    public async Task<Result<ChatResponse>> ProcessStreamingChatRequestAsync(ChatRequest request, CancellationToken cancellationToken = default)
    {
        return await ProcessChatRequestAsync(request, cancellationToken);
    }

    private static IntentType DetectIntent(string prompt)
    {
        var lowerPrompt = prompt.ToLower();

        if (lowerPrompt.Contains("explain") || lowerPrompt.Contains("what is") || lowerPrompt.Contains("how does"))
            return IntentType.Explain;
        if (lowerPrompt.Contains("generate") || lowerPrompt.Contains("create") || lowerPrompt.Contains("write"))
            return IntentType.Generate;
        if (lowerPrompt.Contains("fix") || lowerPrompt.Contains("bug") || lowerPrompt.Contains("error"))
            return IntentType.Fix;
        if (lowerPrompt.Contains("refactor") || lowerPrompt.Contains("improve") || lowerPrompt.Contains("optimize"))
            return IntentType.Refactor;
        if (lowerPrompt.Contains("test") || lowerPrompt.Contains("unit test"))
            return IntentType.Test;
        if (lowerPrompt.Contains("search") || lowerPrompt.Contains("find"))
            return IntentType.Search;
        if (lowerPrompt.Contains("execute") || lowerPrompt.Contains("run"))
            return IntentType.Execute;

        return IntentType.Unknown;
    }

    private static int EstimateTokens(string prompt, string response)
    {
        return (prompt.Length + response.Length) / 4;
    }

    private static string GetErrorChain(Exception ex)
    {
        var parts = new List<string>();
        var current = ex;
        while (current != null)
        {
            parts.Add(current.Message);
            current = current.InnerException;
        }
        return string.Join(" → ", parts);
    }

    private static List<string> GetAllErrors(Exception ex)
    {
        var errors = new List<string>();
        var current = ex;
        while (current != null)
        {
            errors.Add(current.Message);
            current = current.InnerException;
        }
        return errors;
    }
}
