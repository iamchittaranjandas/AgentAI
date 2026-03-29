using AgentAI.Application.Common.Interfaces;
using AgentAI.Application.Interfaces.Infrastructure;
using AgentAI.Application.Interfaces.Repositories;
using AgentAI.Application.Interfaces.Services;
using AgentAI.Infrastructure.AI;
using AgentAI.Infrastructure.Persistence;
using AgentAI.Infrastructure.Repositories;
using AgentAI.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Embeddings;

namespace AgentAI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IPromptRecordRepository, PromptRecordRepository>();
        services.AddScoped<IRetrievalChunkRepository, RetrievalChunkRepository>();
        services.AddScoped<IToolExecutionRepository, ToolExecutionRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IMemoryItemRepository, MemoryItemRepository>();
        services.AddScoped<IRepositoryRepository, RepositoryRepository>();
        services.AddScoped<ICodeFileRepository, CodeFileRepository>();

        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IRetrievalService, RetrievalService>();
        services.AddScoped<IToolExecutionService, ToolExecutionService>();
        services.AddScoped<IIndexingService, IndexingService>();
        services.AddScoped<IAgentOrchestrationService, AgentOrchestrationService>();

        var kernelBuilder = services.AddKernel();
        
        var openRouterKey = configuration["OpenRouter:ApiKey"];
        var openRouterModel = configuration["OpenRouter:Model"] ?? "openai/gpt-4-turbo";
        var embeddingModel = configuration["OpenRouter:EmbeddingModel"] ?? "openai/text-embedding-3-small";
        var openRouterEndpoint = configuration["OpenRouter:Endpoint"] ?? "https://openrouter.ai/api/v1";

        if (!string.IsNullOrEmpty(openRouterKey))
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(openRouterEndpoint)
            };
            httpClient.DefaultRequestHeaders.Add("HTTP-Referer", configuration["OpenRouter:SiteUrl"] ?? "http://localhost:5000");
            httpClient.DefaultRequestHeaders.Add("X-Title", configuration["OpenRouter:SiteName"] ?? "AgentAI");

            kernelBuilder.AddOpenAIChatCompletion(
                modelId: openRouterModel,
                apiKey: openRouterKey,
                httpClient: httpClient);
            
            kernelBuilder.AddOpenAITextEmbeddingGeneration(
                modelId: embeddingModel,
                apiKey: openRouterKey,
                httpClient: httpClient);
        }

        services.AddScoped<ILLMProvider, SemanticKernelLLMProvider>();
        services.AddScoped<IEmbeddingService, EmbeddingService>();

        return services;
    }
}
