using FluentValidation;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using SwiftLink.Application.Common;
using SwiftLink.Application.Common.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace SwiftLink.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationFailures = await Task.WhenAll(_validators.Select(validator => validator.ValidateAsync(context)));

        var errors = validationFailures
           .Where(validationResult => !validationResult.IsValid)
           .SelectMany(validationResult => validationResult.Errors)
           .Select(validationFailure => new ValidationError(validationFailure.PropertyName, validationFailure.ErrorMessage))
           .ToList();

        //if (errors.Count != 0)
        //    throw new BusinessValidationException(errors);

        var response = await next();
        return response;
    }
}