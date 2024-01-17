using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SwiftLink.Application.UseCases.Links.Commmands;
using SwiftLink.Application.UseCases.Links.Queries.VisitShortCode;
using SwiftLink.Presentation.Extensions;
using SwiftLink.Presentation.Filters;

namespace SwiftLink.Presentation.Controllers;

[ApiController]
public class LinkController(ISender sender) : Controller
{
    private readonly ISender _mediarR = sender;

    [HttpPost]
    [Route("api/v{v:apiVersion}/[controller]/[action]")]
    public async Task<IActionResult> Shorten([FromBody] GenerateShortCodeCommand command, CancellationToken cancellationToken = default)
    {
        var response = await _mediarR.Send(command, cancellationToken);
        if (response.IsFailure)
            return Ok(response.MapToProblemDetails());

        return Ok(response);
    }

    [HttpGet, Route("/api/{shortCode}")]
    [ShortenEndpointFilter]
    public async Task<IActionResult> Shorten(string shortCode, [FromQuery] string password, CancellationToken cancellationToken = default)
    {
        var visitLinkQuery = new VisitShortenLinkQuery(shortCode, password, "");
        var response = await _mediarR.Send(visitLinkQuery, cancellationToken);
        if (response.IsFailure)
            return Ok(response.MapToProblemDetails());

        return Ok(response.Data);
    }
}


