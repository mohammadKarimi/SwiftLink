using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SwiftLink.Application.Common.Exceptions;

namespace SwiftLink.Presentation.Middleware;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, CancellationToken, Task>> _exceptionHandlers;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
        _exceptionHandlers = new()
        {
            { typeof(BusinessValidationException), HandleBusinessValidationException },
            { typeof(SubscriberUnAuthorizedException), HandleSubscriberUnAuthorizedException },
        };
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Server Error happened!");

        var exceptionType = exception.GetType();
        if (_exceptionHandlers.TryGetValue(exceptionType,
                out var value))
        {
            await value.Invoke(httpContext, exception, cancellationToken);
            return true;
        }

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response
            .WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server error"
            }, cancellationToken);

        return false;
    }

    private async Task HandleBusinessValidationException(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Validation error happened!");

        var businessValidationException = (BusinessValidationException)exception;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation error",
            Detail = "One or more validation errors has occurred",
        };

        if (businessValidationException.Errors is not null)
            problemDetails.Extensions["errors"] = businessValidationException.Errors;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(problemDetails,
            cancellationToken: cancellationToken);
    }

    private async Task HandleSubscriberUnAuthorizedException(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "UnAuthorized user error happened!");

        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "UnAuthorized User",
            Detail = "Token is not sent or User is unauthorized :(",
        }, cancellationToken: cancellationToken);
    }
}
