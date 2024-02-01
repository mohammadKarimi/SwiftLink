using FluentValidation;
using SwiftLink.Application.Common.Interfaces;

namespace SwiftLink.Application.UseCases.Links.Commands;

public class GenerateShortCodeValidator : AbstractValidator<GenerateShortCodeCommand>
{
    public readonly IApplicationDbContext _dbContext;

    public GenerateShortCodeValidator(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Url)
            .NotNull().WithMessage(LinkMessages.UrlMustBeSent.Message)
            .Must(BeAValidUrl).WithMessage(LinkMessages.InvalidUrlFormat.Message);
    }

    private bool BeAValidUrl(string url)
        => UrlFormatChecker.UrlRegex().IsMatch(url);
}