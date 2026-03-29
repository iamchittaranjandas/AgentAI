using AgentAI.Application.Common;
using AgentAI.Application.DTOs.Session;

namespace AgentAI.Application.Interfaces.Services;

public interface ISessionService
{
    Task<Result<SessionDto>> CreateSessionAsync(CreateSessionRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<Result<SessionDto>> GetSessionByIdAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task<Result<SessionDetailDto>> GetSessionDetailAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task<Result<PaginatedList<SessionDto>>> GetUserSessionsAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Result<SessionDto>> UpdateSessionAsync(Guid sessionId, UpdateSessionRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteSessionAsync(Guid sessionId, CancellationToken cancellationToken = default);
}
