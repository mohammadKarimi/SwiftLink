using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SwiftLink.Application.Common.Exceptions;
using System;

namespace SwiftLink.Presentation.Middleware;

public sealed class BusinessValidationExceptionHandlingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessValidationException exception)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "ValidationFailure",
                Title = "Validation error",
                Detail = "One or more validation errors has occurred"
            };

            if (exception.Errors is not null)
                problemDetails.Extensions["errors"] = exception.Errors;

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}