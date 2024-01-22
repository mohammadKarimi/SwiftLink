using MediatR;
using Microsoft.AspNetCore.Mvc;
using SwiftLink.Application.UseCases.Links.Commands;
using SwiftLink.Application.UseCases.Links.Queries.VisitShortCode;
using SwiftLink.Presentation.Filters;

namespace SwiftLink.Presentation.Controllers;

[Route("api/v{v:apiVersion}/[controller]/[action]")]
public class LinkController(ISender sender) : BaseController(sender)
{
    [HttpPost]
    public async Task<IActionResult> Shorten([FromBody] GenerateShortCodeCommand command, CancellationToken cancellationToken = default)
        => OK(await _mediarR.Send(command, cancellationToken));

    [HttpGet, Route("/api/{shortCode}")] //TODO: this routing should be removed.
    [ShortenEndpointFilter]
    public async Task<IActionResult> Shorten(string shortCode, [FromQuery] string password, CancellationToken cancellationToken = default)
    {
        var response = await _mediarR.Send(new VisitShortenLinkQuery()
        {
            ShortCode = shortCode,
            Password = password,
            ClientMetaData = string.Empty
        }, cancellationToken);

        if (response.IsSuccess)
            return Redirect(response.Data);

        return Ok(response);
    }
}
