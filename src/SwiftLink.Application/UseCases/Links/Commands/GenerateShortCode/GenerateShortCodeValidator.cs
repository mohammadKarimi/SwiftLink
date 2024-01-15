﻿using FluentValidation;
using SwiftLink.Application.Common.Interfaces;
using SwiftLink.Application.UseCases.Subscribers;

namespace SwiftLink.Application.UseCases.Links.Commmands;

public class GenerateShortCodeValidator : AbstractValidator<GenerateShortCodeCommand>
{
    public readonly ISharedContext _sharedContext;
    public readonly IApplicationDbContext _dbContext;

    public GenerateShortCodeValidator(IApplicationDbContext dbContext, ISharedContext sharedContext)
    {
        _dbContext = dbContext;
        _sharedContext = sharedContext;

        RuleFor(x => x.Url)
            .NotNull().WithMessage(LinkMessages.UrlMustBeSent.Message)
            .Must(BeAValidUrl).WithMessage(LinkMessages.InvalidUrlFormat.Message);

        RuleFor(x => x.Token)
            .NotNull().WithMessage(SubscriberMessage.SubscriberMustBeSent.Message)
            .MustAsync(BeAValidSubscriber).WithMessage(SubscriberMessage.SubscriberMustBeSent.Message);
    }

    private bool BeAValidUrl(string url)
        => UrlFormatChecker.UrlRegex().IsMatch(url);

    private async Task<bool> BeAValidSubscriber(Guid token, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Set<Subscriber>().FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
        if (result is null)
            return false;

        _sharedContext.Set(nameof(result.Id), result.Id);
        return true;
    }
}