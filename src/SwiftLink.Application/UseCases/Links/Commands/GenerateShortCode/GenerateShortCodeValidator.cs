using FluentValidation;

namespace SwiftLink.Application.UseCases.Links.Commmands;

public class GenerateShortCodeValidator : AbstractValidator<GenerateShortCodeCommand>
{
    public GenerateShortCodeValidator()
    {
        RuleFor(x => x.Url)
            .NotNull().WithMessage(Constants.Link.UrlMustBeSent.Message)
            .Must(BeAValidUrl).WithMessage(Constants.Link.InvalidUrlFormat.Message);

        //RuleFor(x => x.Token)
        //    .NotNull().WithMessage(Constants.Link.UrlMustBeSent)
        //    .Must(BeAValidUrl).WithMessage(Constants.Link.InvalidUrlFormat);
    }

    private bool BeAValidUrl(string url)
        => UrlFormatChecker.UrlRegex().IsMatch(url);
}
