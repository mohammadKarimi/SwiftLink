using FluentValidation;
using SwiftLink.Application.Resources;

namespace SwiftLink.Application.UseCases.Links.GenerateCommand;

public class GenerateShortCodeValidator : AbstractValidator<GenerateShortCodeCommand>
{
    public GenerateShortCodeValidator()
    {
        RuleFor(x => x.Url).NotNull()
                           .WithMessage(ApplicationMessage.Link.UrlMustBeSent);
    }
}