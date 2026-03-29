using AgentAI.Application.Common;
using AgentAI.Application.DTOs.Tool;
using AgentAI.Application.Interfaces.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgentAI.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class ToolController : ControllerBase
{
    private readonly IToolExecutionService _toolExecutionService;
    private readonly ILogger<ToolController> _logger;

    public ToolController(IToolExecutionService toolExecutionService, ILogger<ToolController> logger)
    {
        _toolExecutionService = toolExecutionService;
        _logger = logger;
    }

    [HttpPost("execute")]
    public async Task<ActionResult<ApiResponse<ToolExecutionResultDto>>> ExecuteTool(
        [FromBody] ToolExecutionRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Tool execution {ToolType}/{Action} by user {UserId}",
            request.ToolType, request.Action,
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

        var result = await _toolExecutionService.ExecuteToolAsync(request, cancellationToken);

        if (!result.Success)
            return BadRequest(new ApiResponse<ToolExecutionResultDto> { Success = false, Errors = result.Errors, Message = result.Message });

        return Ok(new ApiResponse<ToolExecutionResultDto> { Success = true, Data = result.Data, Message = result.Message });
    }

    [HttpPost("{executionId}/approve")]
    public async Task<ActionResult<ApiResponse<ToolExecutionResultDto>>> ApproveTool(
        Guid executionId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Approving tool execution {ExecutionId} by user {UserId}",
            executionId,
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

        var result = await _toolExecutionService.ApproveToolExecutionAsync(executionId, cancellationToken);

        if (!result.Success)
            return BadRequest(new ApiResponse<ToolExecutionResultDto> { Success = false, Errors = result.Errors, Message = result.Message });

        return Ok(new ApiResponse<ToolExecutionResultDto> { Success = true, Data = result.Data, Message = result.Message });
    }

    [HttpPost("{executionId}/reject")]
    public async Task<ActionResult<ApiResponse<object>>> RejectTool(
        Guid executionId,
        [FromBody] RejectToolRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Rejecting tool execution {ExecutionId} by user {UserId}",
            executionId,
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

        var result = await _toolExecutionService.RejectToolExecutionAsync(executionId, request.Reason, cancellationToken);

        if (!result.Success)
            return BadRequest(new ApiResponse<object> { Success = false, Errors = result.Errors, Message = result.Message });

        return Ok(new ApiResponse<object> { Success = true, Message = result.Message });
    }

    [HttpGet("history")]
    public async Task<ActionResult<ApiResponse<PaginatedList<ToolExecutionDto>>>> GetHistory(
        [FromQuery] Guid? sessionId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _toolExecutionService.GetToolExecutionHistoryAsync(sessionId, page, pageSize, cancellationToken);

        if (!result.Success)
            return BadRequest(new ApiResponse<PaginatedList<ToolExecutionDto>> { Success = false, Errors = result.Errors, Message = result.Message });

        return Ok(new ApiResponse<PaginatedList<ToolExecutionDto>> { Success = true, Data = result.Data });
    }
}
