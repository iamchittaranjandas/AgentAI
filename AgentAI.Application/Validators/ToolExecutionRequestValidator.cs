using AgentAI.Application.DTOs.Tool;
using FluentValidation;

namespace AgentAI.Application.Validators;

public class ToolExecutionRequestValidator : AbstractValidator<ToolExecutionRequest>
{
    public ToolExecutionRequestValidator()
    {
        RuleFor(x => x.SessionId)
            .NotEmpty().WithMessage("SessionId is required");

        RuleFor(x => x.ToolType)
            .IsInEnum().WithMessage("Invalid ToolType");

        RuleFor(x => x.Action)
            .NotEmpty().WithMessage("Action is required")
            .MaximumLength(200).WithMessage("Action cannot exceed 200 characters");

        RuleFor(x => x.Parameters)
            .NotNull().WithMessage("Parameters cannot be null");
    }
}
