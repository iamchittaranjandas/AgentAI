using AgentAI.Application.Common;
using AgentAI.Application.DTOs.Tool;
using AgentAI.Application.Interfaces.Repositories;
using AgentAI.Application.Interfaces.Services;
using AgentAI.Domain.Entities;
using AgentAI.Domain.Enums;
using Mapster;
using System.Text.Json;

namespace AgentAI.Infrastructure.Services;

public class ToolExecutionService : IToolExecutionService
{
    private readonly IToolExecutionRepository _toolExecutionRepository;

    public ToolExecutionService(IToolExecutionRepository toolExecutionRepository)
    {
        _toolExecutionRepository = toolExecutionRepository;
    }

    public async Task<Result<ToolExecutionResultDto>> ExecuteToolAsync(ToolExecutionRequest request, CancellationToken cancellationToken = default)
    {
        var riskLevel = DetermineRiskLevel(request.ToolType, request.Action);
        var requiresApproval = !request.AutoApprove && riskLevel >= RiskLevel.Medium;

        var toolExecution = new ToolExecution
        {
            Id = Guid.NewGuid(),
            SessionId = request.SessionId,
            ToolType = request.ToolType,
            ToolName = request.ToolType.ToString(),
            Action = request.Action,
            InputParameters = JsonSerializer.Serialize(request.Parameters),
            ApprovalStatus = requiresApproval ? ApprovalStatus.Pending : ApprovalStatus.AutoApproved,
            RiskLevel = riskLevel,
            RequestedAt = DateTime.UtcNow,
            WasSuccessful = false
        };

        await _toolExecutionRepository.AddAsync(toolExecution, cancellationToken);

        if (!requiresApproval)
        {
            toolExecution.ApprovedAt = DateTime.UtcNow;
            toolExecution.ExecutedAt = DateTime.UtcNow;
            toolExecution.WasSuccessful = true;
            toolExecution.Output = "Tool execution simulated (not implemented)";
            toolExecution.ExecutionDuration = TimeSpan.FromMilliseconds(100);

            await _toolExecutionRepository.UpdateAsync(toolExecution, cancellationToken);
        }

        var result = new ToolExecutionResultDto
        {
            ExecutionId = toolExecution.Id,
            ApprovalStatus = toolExecution.ApprovalStatus,
            RiskLevel = toolExecution.RiskLevel,
            RequiresApproval = requiresApproval,
            Output = toolExecution.Output,
            WasSuccessful = toolExecution.WasSuccessful,
            ExecutionDuration = toolExecution.ExecutionDuration
        };

        return Result<ToolExecutionResultDto>.SuccessResult(result);
    }

    public async Task<Result<ToolExecutionResultDto>> ApproveToolExecutionAsync(Guid executionId, CancellationToken cancellationToken = default)
    {
        var toolExecution = await _toolExecutionRepository.GetByIdAsync(executionId, cancellationToken);
        if (toolExecution == null)
        {
            return Result<ToolExecutionResultDto>.FailureResult("Tool execution not found");
        }

        if (toolExecution.ApprovalStatus != ApprovalStatus.Pending)
        {
            return Result<ToolExecutionResultDto>.FailureResult("Tool execution is not pending approval");
        }

        toolExecution.ApprovalStatus = ApprovalStatus.Approved;
        toolExecution.ApprovedAt = DateTime.UtcNow;
        toolExecution.ExecutedAt = DateTime.UtcNow;
        toolExecution.WasSuccessful = true;
        toolExecution.Output = "Tool execution simulated (not implemented)";
        toolExecution.ExecutionDuration = TimeSpan.FromMilliseconds(100);

        await _toolExecutionRepository.UpdateAsync(toolExecution, cancellationToken);

        var result = new ToolExecutionResultDto
        {
            ExecutionId = toolExecution.Id,
            ApprovalStatus = toolExecution.ApprovalStatus,
            RiskLevel = toolExecution.RiskLevel,
            RequiresApproval = false,
            Output = toolExecution.Output,
            WasSuccessful = toolExecution.WasSuccessful,
            ExecutionDuration = toolExecution.ExecutionDuration
        };

        return Result<ToolExecutionResultDto>.SuccessResult(result, "Tool execution approved and executed");
    }

    public async Task<Result> RejectToolExecutionAsync(Guid executionId, string? reason, CancellationToken cancellationToken = default)
    {
        var toolExecution = await _toolExecutionRepository.GetByIdAsync(executionId, cancellationToken);
        if (toolExecution == null)
        {
            return Result.FailureResult("Tool execution not found");
        }

        if (toolExecution.ApprovalStatus != ApprovalStatus.Pending)
        {
            return Result.FailureResult("Tool execution is not pending approval");
        }

        toolExecution.ApprovalStatus = ApprovalStatus.Rejected;
        toolExecution.ErrorMessage = reason ?? "Rejected by user";

        await _toolExecutionRepository.UpdateAsync(toolExecution, cancellationToken);

        return Result.SuccessResult("Tool execution rejected");
    }

    public async Task<Result<PaginatedList<ToolExecutionDto>>> GetToolExecutionHistoryAsync(Guid? sessionId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        if (!sessionId.HasValue)
        {
            return Result<PaginatedList<ToolExecutionDto>>.FailureResult("SessionId is required");
        }

        var executions = await _toolExecutionRepository.GetBySessionIdAsync(sessionId.Value, page, pageSize, cancellationToken);
        var dtos = new PaginatedList<ToolExecutionDto>(
            executions.Items.Adapt<List<ToolExecutionDto>>(),
            executions.TotalCount,
            executions.Page,
            executions.PageSize
        );

        return Result<PaginatedList<ToolExecutionDto>>.SuccessResult(dtos);
    }

    private static RiskLevel DetermineRiskLevel(ToolType toolType, string action)
    {
        return toolType switch
        {
            ToolType.File when action.Contains("delete", StringComparison.OrdinalIgnoreCase) => RiskLevel.High,
            ToolType.File when action.Contains("write", StringComparison.OrdinalIgnoreCase) => RiskLevel.Medium,
            ToolType.Git when action.Contains("push", StringComparison.OrdinalIgnoreCase) => RiskLevel.High,
            ToolType.Database => RiskLevel.Critical,
            ToolType.Build => RiskLevel.Low,
            ToolType.Test => RiskLevel.Low,
            _ => RiskLevel.Medium
        };
    }
}
