using AgentAI.Application.Common;
using AgentAI.Application.DTOs.Chat;
using AgentAI.Application.DTOs.Session;
using AgentAI.Application.Interfaces.Repositories;
using AgentAI.Application.Interfaces.Services;
using AgentAI.Domain.Entities;
using AgentAI.Domain.Enums;
using Mapster;

namespace AgentAI.Infrastructure.Services;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IPromptRecordRepository _promptRecordRepository;

    public SessionService(
        ISessionRepository sessionRepository,
        IPromptRecordRepository promptRecordRepository)
    {
        _sessionRepository = sessionRepository;
        _promptRecordRepository = promptRecordRepository;
    }

    public async Task<Result<SessionDto>> CreateSessionAsync(CreateSessionRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        var session = new Session
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = request.Title ?? $"Session {DateTime.UtcNow:yyyy-MM-dd HH:mm}",
            Status = SessionStatus.Active,
            RepositoryPath = request.RepositoryPath,
            Branch = request.Branch,
            StartedAt = DateTime.UtcNow,
            LastActivityAt = DateTime.UtcNow,
            MessageCount = 0
        };

        await _sessionRepository.AddAsync(session, cancellationToken);

        var dto = session.Adapt<SessionDto>();
        return Result<SessionDto>.SuccessResult(dto, "Session created successfully");
    }

    public async Task<Result<SessionDto>> GetSessionByIdAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId, cancellationToken);
        if (session == null)
        {
            return Result<SessionDto>.FailureResult("Session not found");
        }

        var dto = session.Adapt<SessionDto>();
        return Result<SessionDto>.SuccessResult(dto);
    }

    public async Task<Result<SessionDetailDto>> GetSessionDetailAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetByIdWithDetailsAsync(sessionId, cancellationToken);
        if (session == null)
        {
            return Result<SessionDetailDto>.FailureResult("Session not found");
        }

        var dto = new SessionDetailDto
        {
            Id = session.Id,
            Title = session.Title,
            Status = session.Status,
            RepositoryPath = session.RepositoryPath,
            StartedAt = session.StartedAt,
            LastActivityAt = session.LastActivityAt,
            MessageCount = session.MessageCount,
            Messages = session.PromptRecords.Adapt<List<MessageDto>>()
        };

        return Result<SessionDetailDto>.SuccessResult(dto);
    }

    public async Task<Result<PaginatedList<SessionDto>>> GetUserSessionsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var sessions = await _sessionRepository.GetByUserIdAsync(userId, page, pageSize, cancellationToken);
        var dtos = new PaginatedList<SessionDto>(
            sessions.Items.Adapt<List<SessionDto>>(),
            sessions.TotalCount,
            sessions.Page,
            sessions.PageSize
        );

        return Result<PaginatedList<SessionDto>>.SuccessResult(dtos);
    }

    public async Task<Result<SessionDto>> UpdateSessionAsync(Guid sessionId, UpdateSessionRequest request, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId, cancellationToken);
        if (session == null)
        {
            return Result<SessionDto>.FailureResult("Session not found");
        }

        if (!string.IsNullOrEmpty(request.Title))
        {
            session.Title = request.Title;
        }

        if (request.Status.HasValue)
        {
            session.Status = request.Status.Value;
        }

        await _sessionRepository.UpdateAsync(session, cancellationToken);

        var dto = session.Adapt<SessionDto>();
        return Result<SessionDto>.SuccessResult(dto, "Session updated successfully");
    }

    public async Task<Result> DeleteSessionAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId, cancellationToken);
        if (session == null)
        {
            return Result.FailureResult("Session not found");
        }

        await _sessionRepository.DeleteAsync(session, cancellationToken);
        return Result.SuccessResult("Session deleted successfully");
    }
}
