using Microsoft.AspNetCore.Mvc;
using SwiftLink.Shared;

namespace SwiftLink.Presentation.Extensions;

public static class MapToProblemDetailsExtension
{
    public static ProblemDetails MapToProblemDetails(this Result result)
    {
        static int GetStatusCode(ErrorType type)
            => type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.None => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };

        static string GetTitle(ErrorType type)
           => type switch
           {
               ErrorType.Validation => "Bad Request",
               ErrorType.Failure => "Internal Server Error",
               ErrorType.NotFound => "Not Found",
               ErrorType.None => "Internal Server Error",
               _ => "Internal Server Error"
           };

        return new()
        {
            Status = GetStatusCode(result.Error.Type),
            Type = result.Error.Type.ToString(),
            Title = GetTitle(result.Error.Type),
            Detail = result.Error.Message
        };
    }
}
