using FluentValidation;
using SwiftLink.Application.Common.Interfaces;

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
            .Must(BeAValidCharacters).WithMessage(LinkMessages.BackHalfInvalidFormat.Message)
            .MustAsync(BeAValidBackHalf).WithMessage(LinkMessages.BackHalfIsExist.Message);

        RuleFor(x => x.ExpirationDate)
            .Must(BeAValidExpirationDate)
            .WithMessage(LinkMessages.ExpirationDateMustBeMoreThanTomorrow.Message)
             .NotNull()
             .When(x => x.RemindDate is not null)
             .WithMessage(LinkMessages.ExpirationDateIsRequiredToSetReminder.Message);

        RuleFor(x => new { x.RemindDate, x.ExpirationDate })
            .Must(x => BeInRightInterval(x.RemindDate.Value, x.ExpirationDate.Value))
            .WithMessage(LinkMessages.InvalidReminderDate.Message)
            .When(x => x.RemindDate is not null);
    }

    private bool BeAValidUrl(string url)
       => UrlFormatChecker.UrlRegex().IsMatch(url);

    private bool BeAValidCharacters(string backHalf)
      => BackHalfFormatChecker.WordRegex().IsMatch(backHalf);

    private bool BeAValidExpirationDate(DateTime? date)
        => date is null || date.Value > DateTime.Now;

    private bool BeInRightInterval(DateTime remindDate, DateTime expirationDate)
        => DateTime.Now < remindDate &&
           remindDate < expirationDate;

    private async Task<bool> BeAValidBackHalf(string backHalf, CancellationToken cancellationToken)
        => backHalf is null || !await _dbContext.Set<Link>().AnyAsync(x => x.ShortCode == backHalf, cancellationToken);
}
