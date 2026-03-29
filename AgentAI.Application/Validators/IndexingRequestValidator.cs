using AgentAI.Application.DTOs.Indexing;
using FluentValidation;

namespace AgentAI.Application.Validators;

public class IndexingRequestValidator : AbstractValidator<IndexingRequest>
{
    public IndexingRequestValidator()
    {
        RuleFor(x => x.RepositoryPath)
            .NotEmpty().WithMessage("RepositoryPath is required")
            .MaximumLength(1000).WithMessage("RepositoryPath cannot exceed 1000 characters");

        RuleFor(x => x.Branch)
            .MaximumLength(200).WithMessage("Branch cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.Branch));
    }
}
