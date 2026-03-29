using AgentAI.Application.Common;
using AgentAI.Application.DTOs.Auth;
using AgentAI.Application.Interfaces.Services;
using AgentAI.Domain.Entities;
using AgentAI.Domain.Enums;
using AgentAI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AgentAI.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(ApplicationDbContext db, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _db = db;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (await _db.Users.AnyAsync(u => u.Email == request.Email.ToLower(), cancellationToken))
            return Result<AuthResponse>.FailureResult("Email is already registered.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email.ToLower(),
            FullName = request.FullName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.Developer,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("New user registered: {Email}", user.Email);

        var (accessToken, expiresAt) = GenerateAccessToken(user);
        var refreshToken = await SetRefreshTokenAsync(user, cancellationToken);

        return Result<AuthResponse>.SuccessResult(BuildAuthResponse(user, accessToken, refreshToken, expiresAt));
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email.ToLower(), cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result<AuthResponse>.FailureResult("Invalid email or password.");

        if (!user.IsActive)
            return Result<AuthResponse>.FailureResult("Account is disabled.");

        _logger.LogInformation("User logged in: {Email}", user.Email);

        var (accessToken, expiresAt) = GenerateAccessToken(user);
        var refreshToken = await SetRefreshTokenAsync(user, cancellationToken);

        return Result<AuthResponse>.SuccessResult(BuildAuthResponse(user, accessToken, refreshToken, expiresAt));
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(
            u => u.RefreshToken == refreshToken && u.RefreshTokenExpiresAt > DateTime.UtcNow,
            cancellationToken);

        if (user == null)
            return Result<AuthResponse>.FailureResult("Invalid or expired refresh token.");

        var (accessToken, expiresAt) = GenerateAccessToken(user);
        var newRefreshToken = await SetRefreshTokenAsync(user, cancellationToken);

        return Result<AuthResponse>.SuccessResult(BuildAuthResponse(user, accessToken, newRefreshToken, expiresAt));
    }

    public async Task<Result> RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, cancellationToken);

        if (user == null)
            return Result.FailureResult("Token not found.");

        user.RefreshToken = null;
        user.RefreshTokenExpiresAt = null;
        await _db.SaveChangesAsync(cancellationToken);

        return Result.SuccessResult();
    }

    private (string token, DateTime expiresAt) GenerateAccessToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var expiryMinutes = int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60");
        var expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }

    private async Task<string> SetRefreshTokenAsync(User user, CancellationToken cancellationToken)
    {
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var expiryDays = int.Parse(_configuration["Jwt:RefreshTokenExpiryDays"] ?? "7");

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(expiryDays);
        await _db.SaveChangesAsync(cancellationToken);

        return refreshToken;
    }

    private static AuthResponse BuildAuthResponse(User user, string accessToken, string refreshToken, DateTime expiresAt) =>
        new()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt,
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString()
        };
}
