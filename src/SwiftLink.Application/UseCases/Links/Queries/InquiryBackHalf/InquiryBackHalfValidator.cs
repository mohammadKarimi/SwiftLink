using FluentValidation;

namespace SwiftLink.Application.UseCases.Links.Queries;
public class InquiryBackHalfValidator : AbstractValidator<InquiryBackHalfQuery>
{
    public InquiryBackHalfValidator()
    {
        RuleFor(x => x.BackHalfText)
            .NotNull().WithMessage(LinkMessages.InvalidBackHalf.Message)
            .MaximumLength(30).WithMessage(LinkMessages.InvalidBackHalfLength.Message);
    }
}
