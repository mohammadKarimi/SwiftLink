using Azure;
using FluentValidation;
using MediatR;
using System.Net;

namespace SwiftLink.Application.Behaviors;

/// <summary>
/// Pipie line for Validation.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResult"></typeparam>
public class ValidationBehavior<TRequest, TResult>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResult> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next,
        CancellationToken ct = default)
    {
        if (!_validators.Any())
            return await next();

        var validationContext = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(validationContext, ct)));

        var errorList = validationResults.Where(r => r.Errors.Count != 0)
                                         .SelectMany(x => x.Errors)
                                         .ToList();

        if (errorList.Count > 0)
        {
            var message = string.Join(" | ", errorList.Select(e => e.ErrorMessage));

            if (Enum.TryParse(errorList.First().ErrorCode, out HttpStatusCode statusCode))
                return GenerateResponse(message, statusCode);

            return GenerateResponse(message);
        }

        return await next();
    }

    private static TResult GenerateResponse(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
       => (TResult)Activator.CreateInstance(typeof(Result<object>), false, message, statusCode);
}
