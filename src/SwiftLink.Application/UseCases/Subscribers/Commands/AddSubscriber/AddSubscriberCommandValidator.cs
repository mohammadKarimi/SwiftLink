using FluentValidation;
using SwiftLink.Application.Common.Interfaces;

namespace SwiftLink.Application.UseCases.Subscribers.Commands;

public class AddSubscriberCommandValidator : AbstractValidator<AddSubscriberCommand>
{
    public readonly IApplicationDbContext _dbContext;

    public AddSubscriberCommandValidator(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Name)
            .NotNull().WithMessage(SubscriberMessage.NameMustBeSent.Message);

        RuleFor(x => x.Email)
           .NotNull().WithMessage(SubscriberMessage.EmailMustBeSent.Message)
           .Must(BeAValidEmail).WithMessage(SubscriberMessage.EmailFormatIsIncorrect.Message)
           .MustAsync(BeAUniqueEmail).WithMessage(SubscriberMessage.EmailMustBeUnique.Message);
    }

    private bool BeAValidEmail(string email)
      => EmailFormatChecker.EmailRegex().IsMatch(email);

    private async Task<bool> BeAUniqueEmail(string email, CancellationToken cancellationToken)
       => !await _dbContext.Set<Subscriber>().AnyAsync(x => x.Email == email, cancellationToken);
}
