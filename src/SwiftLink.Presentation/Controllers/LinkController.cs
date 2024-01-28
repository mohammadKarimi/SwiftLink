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
    [HeaderExtraction]
    public async Task<IActionResult> Visit(string shortCode, [FromQuery] string password,
        CancellationToken cancellationToken = default)
    {

        HttpContext.Items.TryGetValue("ClientMetaData", out object clientMetaData);
        var response = await _mediatR.Send(new VisitShortenLinkQuery()
        {
            ShortCode = shortCode,
            Password = password,
            ClientMetaData = clientMetaData.ToString()
        }, cancellationToken);

        if (response.IsSuccess)
            return Redirect(response.Data);

        return Ok(response);
    }
}