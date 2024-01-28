using MediatR;
using Microsoft.AspNetCore.Mvc;
using SwiftLink.Application.UseCases.Links.Commands.GenerateShortCode;
using SwiftLink.Application.UseCases.Links.Queries.VisitShortenLink;
using SwiftLink.Presentation.Filters;
using System.Text.Json;

namespace SwiftLink.Presentation.Controllers;

[Route("api/v{v:apiVersion}/[controller]/[action]")]
public class LinkController(ISender sender) : BaseController(sender)
{
    [HttpPost]
    public async Task<IActionResult> Shorten([FromBody] GenerateShortCodeCommand command,
        CancellationToken cancellationToken = default)
        => OK(await _mediatR.Send(command, cancellationToken));

    [HttpGet, Route("/link/{shortCode}")]
    [ShortenEndpointFilter]
    public async Task<IActionResult> Visit(string shortCode, [FromQuery] string password,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediatR.Send(new VisitShortenLinkQuery()
        {
            ShortCode = shortCode,
            Password = password,
            ClientMetaData = JsonSerializer.Serialize(new
            {
                OperationSystem = HttpContext.Request.Headers["sec-ch-ua-platform"],
                Mobile = HttpContext.Request.Headers["sec-ch-ua-mobile"],
                HttpContext.Request.Headers.UserAgent,
                Browser = HttpContext.Request.Headers["sec-ch-ua"]
            })
        }, cancellationToken);

        if (response.IsSuccess)
            return Redirect(response.Data);

        return Ok(response);
    }
}