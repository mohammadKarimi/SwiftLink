using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SwiftLink.Application.UseCases.Links.Commmands;
using SwiftLink.Presentation.Extensions;
using SwiftLink.Presentation.Filters;

namespace SwiftLink.Presentation.Controllers;

[ApiVersion("1.0")]
public class LinkController : BaseController
{
    [HttpPost]
    [ShortenEndpointFilter]
    public async Task<IActionResult> Shorten([FromBody] GenerateShortCodeCommand command, CancellationToken cancellationToken = default)
    {
        var response = await MediatR.Send(command, cancellationToken);
        if (response.IsFailure)
            return Ok(response.MapToProblemDetails());

        return Ok(response);
    }
}


