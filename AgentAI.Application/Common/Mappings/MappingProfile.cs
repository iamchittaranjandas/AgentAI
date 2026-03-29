using AgentAI.Application.DTOs.Session;
using AgentAI.Domain.Entities;
using AutoMapper;

namespace AgentAI.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Session, SessionDto>();
        CreateMap<Session, SessionDetailDto>()
            .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.PromptRecords));
        
        CreateMap<PromptRecord, PromptRecordDto>();
        CreateMap<ToolExecution, ToolExecutionDto>();
        CreateMap<RetrievalChunk, RetrievalChunkDto>();
        CreateMap<User, UserDto>();
    }
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class PromptRecordDto
{
    public Guid Id { get; set; }
    public string UserPrompt { get; set; } = string.Empty;
    public string AssistantResponse { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class ToolExecutionDto
{
    public Guid Id { get; set; }
    public string ToolType { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class RetrievalChunkDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public double SimilarityScore { get; set; }
}
