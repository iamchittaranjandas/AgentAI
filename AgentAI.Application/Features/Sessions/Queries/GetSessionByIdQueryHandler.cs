using AgentAI.Application.Common.Exceptions;
using AgentAI.Application.Common.Models;
using AgentAI.Application.DTOs.Session;
using AgentAI.Application.Interfaces.Repositories;
using AgentAI.Domain.Entities;
using AutoMapper;
using MediatR;

namespace AgentAI.Application.Features.Sessions.Queries;

public class GetSessionByIdQueryHandler : IRequestHandler<GetSessionByIdQuery, Result<SessionDto>>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IMapper _mapper;

    public GetSessionByIdQueryHandler(
        ISessionRepository sessionRepository,
        IMapper mapper)
    {
        _sessionRepository = sessionRepository;
        _mapper = mapper;
    }

    public async Task<Result<SessionDto>> Handle(GetSessionByIdQuery request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByIdAsync(request.SessionId, cancellationToken);

        if (session == null)
        {
            throw new NotFoundException(nameof(Session), request.SessionId);
        }

        var sessionDto = _mapper.Map<SessionDto>(session);

        return Result<SessionDto>.SuccessResult(sessionDto);
    }
}
