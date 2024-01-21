using Azure;
using Microsoft.AspNetCore.Mvc;
using SwiftLink.Presentation.Extensions;
using SwiftLink.Shared;

namespace SwiftLink.Presentation.Controllers;

[ApiController]
public abstract class BaseController : Controller
{
    protected IActionResult OK<T>(Result<T> response)
    {
        if (response.IsFailure)
            return Ok(response.MapToProblemDetails());

        return Ok(response);
    }
}
