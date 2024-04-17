using FluentValidation;

namespace SwiftLink.Application.UseCases.LinkVisits.Queries;

public class GetClientMetaDateByLinkIdQueryValidator : AbstractValidator<GetClientMetaDataByLinkIdQuery>
{
    public GetClientMetaDateByLinkIdQueryValidator()

    {
        RuleFor(x => x.LinkId)
            .NotNull().WithMessage(LinkVisitMessages.LinkIdMustBeSent.Message);
    }
}
