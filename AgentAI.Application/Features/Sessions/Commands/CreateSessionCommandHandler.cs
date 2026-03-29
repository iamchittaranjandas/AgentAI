using AgentAI.Application.Common.Interfaces;
using AgentAI.Application.Common.Models;
using AgentAI.Application.DTOs.Session;
using AgentAI.Application.Interfaces.Repositories;
using AgentAI.Domain.Entities;
using AgentAI.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AgentAI.Application.Features.Sessions.Commands;

public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, Result<SessionDto>>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateSessionCommandHandler(
        ISessionRepository sessionRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<SessionDto>> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var session = new Session
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Title,
            RepositoryPath = request.RepositoryPath,
            Branch = request.Branch,
            Status = SessionStatus.Active,
            StartedAt = DateTime.UtcNow,
            LastActivityAt = DateTime.UtcNow,
            MessageCount = 0,
            CreatedAt = DateTime.UtcNow
        };

        await _sessionRepository.AddAsync(session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var sessionDto = _mapper.Map<SessionDto>(session);

        return Result<SessionDto>.SuccessResult(sessionDto, "Session created successfully");
    }
}
