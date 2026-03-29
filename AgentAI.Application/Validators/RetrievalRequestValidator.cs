using AgentAI.Application.DTOs.Retrieval;
using FluentValidation;

namespace AgentAI.Application.Validators;

public class RetrievalRequestValidator : AbstractValidator<RetrievalRequest>
{
    public RetrievalRequestValidator()
    {
        RuleFor(x => x.Query)
            .NotEmpty().WithMessage("Query is required")
            .MaximumLength(5000).WithMessage("Query cannot exceed 5000 characters");

        RuleFor(x => x.MaxResults)
            .GreaterThan(0).WithMessage("MaxResults must be greater than 0")
            .LessThanOrEqualTo(50).WithMessage("MaxResults cannot exceed 50");

        RuleFor(x => x.MinSimilarity)
            .GreaterThanOrEqualTo(0).WithMessage("MinSimilarity must be between 0 and 1")
            .LessThanOrEqualTo(1).WithMessage("MinSimilarity must be between 0 and 1");
    }
}
