using MediatR;
using Microsoft.AspNetCore.Mvc;
using SwiftLink.Presentation.Extensions;
using SwiftLink.Shared;

namespace SwiftLink.Presentation.Controllers;

[Route("api/v{v:apiVersion}/[controller]/[action]")]
[ApiController]
public abstract class BaseController(ISender sender) : Controller
{
    protected readonly ISender MediatR = sender;

    protected IActionResult OK<T>(Result<T> response)
       => response.IsSuccess || response.Error.Type == ErrorType.None ? Ok(response) : (IActionResult)Ok(response.MapToProblemDetails());
}
