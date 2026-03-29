using AgentAI.Application.DTOs.Session;
using FluentValidation;

namespace AgentAI.Application.Validators;

public class CreateSessionRequestValidator : AbstractValidator<CreateSessionRequest>
{
    public CreateSessionRequestValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(500).WithMessage("Title cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.RepositoryPath)
            .MaximumLength(1000).WithMessage("RepositoryPath cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.RepositoryPath));

        RuleFor(x => x.Branch)
            .MaximumLength(200).WithMessage("Branch cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.Branch));
    }
}
