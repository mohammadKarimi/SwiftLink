using FluentValidation;

namespace SwiftLink.Application.UseCases.Links.Queries.GetByGroupName;
public class GetLinkByGroupNameQueryValidator : AbstractValidator<GetLinkByGroupNameQuery>
{
    public GetLinkByGroupNameQueryValidator()
    {
        RuleFor(x => x.GroupName)
          .NotNull().WithMessage(LinkMessages.GroupNameMustBeSent.Message);
    }
}
