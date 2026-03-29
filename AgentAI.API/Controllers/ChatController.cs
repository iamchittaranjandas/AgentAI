using AgentAI.Application.Common;
using AgentAI.Application.DTOs.Chat;
using AgentAI.Application.Interfaces.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgentAI.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IAgentOrchestrationService _orchestrationService;
    private readonly ILogger<ChatController> _logger;

    public ChatController(IAgentOrchestrationService orchestrationService, ILogger<ChatController> logger)
    {
        _orchestrationService = orchestrationService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ChatResponse>>> Chat(
        [FromBody] ChatRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Chat request from user {UserId} for session {SessionId}",
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
            request.SessionId);

        var result = await _orchestrationService.ProcessChatRequestAsync(request, cancellationToken);

        if (!result.Success)
            return BadRequest(new ApiResponse<ChatResponse> { Success = false, Errors = result.Errors, Message = result.Message });

        return Ok(new ApiResponse<ChatResponse> { Success = true, Data = result.Data, Message = result.Message });
    }

    [HttpPost("stream")]
    public async Task<ActionResult<ApiResponse<ChatResponse>>> StreamChat(
        [FromBody] ChatRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stream chat request from user {UserId} for session {SessionId}",
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
            request.SessionId);

        var result = await _orchestrationService.ProcessStreamingChatRequestAsync(request, cancellationToken);

        if (!result.Success)
            return BadRequest(new ApiResponse<ChatResponse> { Success = false, Errors = result.Errors, Message = result.Message });

        return Ok(new ApiResponse<ChatResponse> { Success = true, Data = result.Data, Message = result.Message });
    }
}
