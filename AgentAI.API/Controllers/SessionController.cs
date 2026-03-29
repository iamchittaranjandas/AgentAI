using AgentAI.Application.Common;
using AgentAI.Application.DTOs.Session;
using AgentAI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgentAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;
    private readonly ILogger<SessionController> _logger;

    public SessionController(
        ISessionService sessionService,
        ILogger<SessionController> logger)
    {
        _sessionService = sessionService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SessionDto>>> CreateSession([FromBody] CreateSessionRequest request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        
        _logger.LogInformation("Creating new session for user {UserId}", userId);

        var result = await _sessionService.CreateSessionAsync(request, userId, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(new ApiResponse<SessionDto>
            {
                Success = false,
                Errors = result.Errors,
                Message = result.Message
            });
        }

        return CreatedAtAction(nameof(GetSession), new { id = result.Data!.Id }, new ApiResponse<SessionDto>
        {
            Success = true,
            Data = result.Data,
            Message = result.Message
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<SessionDto>>> GetSession(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sessionService.GetSessionByIdAsync(id, cancellationToken);

        if (!result.Success)
        {
            return NotFound(new ApiResponse<SessionDto>
            {
                Success = false,
                Errors = result.Errors,
                Message = result.Message
            });
        }

        return Ok(new ApiResponse<SessionDto>
        {
            Success = true,
            Data = result.Data
        });
    }

    [HttpGet("{id}/detail")]
    public async Task<ActionResult<ApiResponse<SessionDetailDto>>> GetSessionDetail(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sessionService.GetSessionDetailAsync(id, cancellationToken);

        if (!result.Success)
        {
            return NotFound(new ApiResponse<SessionDetailDto>
            {
                Success = false,
                Errors = result.Errors,
                Message = result.Message
            });
        }

        return Ok(new ApiResponse<SessionDetailDto>
        {
            Success = true,
            Data = result.Data
        });
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<ApiResponse<PaginatedList<SessionDto>>>> GetUserSessions(
        Guid userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _sessionService.GetUserSessionsAsync(userId, page, pageSize, cancellationToken);

        return Ok(new ApiResponse<PaginatedList<SessionDto>>
        {
            Success = true,
            Data = result.Data
        });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<SessionDto>>> UpdateSession(
        Guid id,
        [FromBody] UpdateSessionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sessionService.UpdateSessionAsync(id, request, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(new ApiResponse<SessionDto>
            {
                Success = false,
                Errors = result.Errors,
                Message = result.Message
            });
        }

        return Ok(new ApiResponse<SessionDto>
        {
            Success = true,
            Data = result.Data,
            Message = result.Message
        });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteSession(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sessionService.DeleteSessionAsync(id, cancellationToken);

        if (!result.Success)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Errors = result.Errors,
                Message = result.Message
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = result.Message
        });
    }
}
