using AgentAI.Application.DTOs.Chat;
using FluentValidation;

namespace AgentAI.Application.Validators;

public class ChatRequestValidator : AbstractValidator<ChatRequest>
{
    public ChatRequestValidator()
    {
        RuleFor(x => x.Prompt)
            .NotEmpty().WithMessage("Prompt is required")
            .MaximumLength(10000).WithMessage("Prompt cannot exceed 10000 characters");

        RuleFor(x => x.SessionId)
            .NotEmpty().WithMessage("SessionId is required");

        RuleFor(x => x.MaxContextChunks)
            .GreaterThan(0).WithMessage("MaxContextChunks must be greater than 0")
            .LessThanOrEqualTo(20).WithMessage("MaxContextChunks cannot exceed 20");
    }
}
