using AgentAI.Application.Common;
using AgentAI.Application.DTOs.Auth;

namespace AgentAI.Application.Interfaces.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<Result> RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
