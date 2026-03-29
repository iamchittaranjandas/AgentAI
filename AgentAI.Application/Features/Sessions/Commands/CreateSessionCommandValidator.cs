using FluentValidation;

namespace AgentAI.Application.Features.Sessions.Commands;

public class CreateSessionCommandValidator : AbstractValidator<CreateSessionCommand>
{
    public CreateSessionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.RepositoryPath)
            .MaximumLength(500).WithMessage("Repository path must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.RepositoryPath));

        RuleFor(x => x.Branch)
            .MaximumLength(100).WithMessage("Branch must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Branch));
    }
}
