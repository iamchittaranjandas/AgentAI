using AgentAI.Application.Common;
using AgentAI.Application.DTOs.Auth;
using AgentAI.Application.Interfaces.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgentAI.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>Register a new user account.</summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request, cancellationToken);

        if (!result.Success)
            return BadRequest(new ApiResponse<AuthResponse> { Success = false, Errors = result.Errors });

        return CreatedAtAction(nameof(Register), new ApiResponse<AuthResponse>
        {
            Success = true,
            Data = result.Data,
            Message = "Registration successful."
        });
    }

    /// <summary>Login and receive JWT access + refresh tokens.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request, cancellationToken);

        if (!result.Success)
            return Unauthorized(new ApiResponse<AuthResponse> { Success = false, Errors = result.Errors });

        return Ok(new ApiResponse<AuthResponse>
        {
            Success = true,
            Data = result.Data,
            Message = "Login successful."
        });
    }

    /// <summary>Exchange a refresh token for a new access token.</summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Refresh(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authService.RefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (!result.Success)
            return Unauthorized(new ApiResponse<AuthResponse> { Success = false, Errors = result.Errors });

        return Ok(new ApiResponse<AuthResponse>
        {
            Success = true,
            Data = result.Data,
            Message = "Token refreshed."
        });
    }

    /// <summary>Revoke a refresh token (logout).</summary>
    [HttpPost("revoke")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<object>>> Revoke(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authService.RevokeTokenAsync(request.RefreshToken, cancellationToken);

        if (!result.Success)
            return BadRequest(new ApiResponse<object> { Success = false, Errors = result.Errors });

        return Ok(new ApiResponse<object> { Success = true, Message = "Token revoked." });
    }

    /// <summary>Get current authenticated user info from token claims.</summary>
    [HttpGet("me")]
    [Authorize]
    public ActionResult<ApiResponse<object>> Me()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var name = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Data = new { userId, email, name, role }
        });
    }
}
