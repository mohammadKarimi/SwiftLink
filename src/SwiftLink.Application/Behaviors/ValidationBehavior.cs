using FluentValidation;
using SwiftLink.Application.Common;
using SwiftLink.Application.Common.Exceptions;

namespace SwiftLink.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationFailures =
            await Task.WhenAll(_validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var errors = validationFailures
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure =>
                new ValidationError(validationFailure.PropertyName, validationFailure.ErrorMessage))
            .Distinct()
            .ToList();

        return errors.Count != 0 ? throw new BusinessValidationException(errors) : await next();
    }
}
