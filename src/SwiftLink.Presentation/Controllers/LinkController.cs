using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SwiftLink.Application.UseCases.Links.Commmands;
using SwiftLink.Application.UseCases.Links.Queries.VisitShortCode;
using SwiftLink.Presentation.Extensions;
using SwiftLink.Presentation.Filters;

namespace SwiftLink.Presentation.Controllers;

public class LinkController : BaseController
{
    [HttpPost]
    [ApiVersion("1.0")]
    public async Task<IActionResult> Shorten([FromBody] GenerateShortCodeCommand command, CancellationToken cancellationToken = default)
    {
        var response = await MediatR.Send(command, cancellationToken);
        if (response.IsFailure)
            return Ok(response.MapToProblemDetails());

        return Ok(response);
    }

    [HttpGet, Route("/x/{shortCode}")]
    [ShortenEndpointFilter]
    public async Task<IActionResult> Shorten(string shortCode, [FromQuery] string password, CancellationToken cancellationToken = default)
    {
        var visitLinkQuery = new VisitShortenLinkQuery(shortCode, password, "");
        var response = await MediatR.Send(visitLinkQuery, cancellationToken);
        if (response.IsFailure)
            return Ok(response.MapToProblemDetails());

        return Redirect(response.Data);
    }
}


