using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
using SwiftLink.Presentation.Extensions;
using SwiftLink.Shared;

namespace SwiftLink.Presentation.Controllers;

[ApiController]
public abstract class BaseController(ISender sender, ILogger<BaseController> logger) : Controller
{
    private readonly ILogger<BaseController> _logger = logger;
    protected readonly ISender _mediatR = sender;

    protected IActionResult OK<T>(Result<T> response)
    {
        if (response.IsFailure)
        {
            using (LogContext.PushProperty("Error", response.Error, true))
                _logger.LogError("Error => {Details}", response.MapToProblemDetails());

            return Ok(response.MapToProblemDetails());
        }

        return Ok(response);
    }
}