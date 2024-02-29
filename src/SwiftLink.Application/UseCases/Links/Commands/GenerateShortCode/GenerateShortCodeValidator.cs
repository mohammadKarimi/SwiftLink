using FluentValidation;
using SwiftLink.Application.Common.Interfaces;
using SwiftLink.Domain.Entities.Link;

namespace SwiftLink.Application.UseCases.Links.Commands;

public class GenerateShortCodeValidator : AbstractValidator<GenerateShortCodeCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public GenerateShortCodeValidator(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Url)
            .NotNull().WithMessage(LinkMessages.UrlMustBeSent.Message)
            .Must(BeAValidUrl).WithMessage(LinkMessages.InvalidUrlFormat.Message);

        RuleFor(x => x.BackHalf)
            .MaximumLength(16).WithMessage(LinkMessages.BackHalfLength.Message)
            .Must(BeAValidCharacters).WithMessage(LinkMessages.BackHalfInvalidFormat.Message).When(x => x.BackHalf is not null)
            .MustAsync(BeAValidBackHalf).WithMessage(LinkMessages.BackHalfIsExist.Message);

        RuleFor(x => x.ExpirationDate)
           .Must(BeAValidExpirationDate).WithMessage(LinkMessages.ExpirationDateMustBeMoreThanTomorrow.Message);
    }

    private bool BeAValidUrl(string url)
       => UrlFormatChecker.UrlRegex().IsMatch(url);

    private bool BeAValidCharacters(string backHalf)
      => BackHalfFormatChecker.WordRegex().IsMatch(backHalf);

    private bool BeAValidExpirationDate(DateTime? date)
        => date is null || date.Value > DateTime.Now;

    private async Task<bool> BeAValidBackHalf(string backHalf, CancellationToken cancellationToken)
        => backHalf is null || !await _dbContext.Set<Link>().AnyAsync(x => x.ShortCode == backHalf, cancellationToken);
}
