using MediatR;
using Microsoft.AspNetCore.Mvc;
using SwiftLink.Application.UseCases.Links.Commands;
using SwiftLink.Application.UseCases.Links.Commands.DisableLink;
using SwiftLink.Application.UseCases.Links.Queries;
using SwiftLink.Application.UseCases.Subscribers.Queries;
using SwiftLink.Presentation.Filters;

namespace SwiftLink.Presentation.Controllers.V1;

public class LinkController(ISender sender) : BaseController(sender)
{
    [HttpPost]
    public async Task<IActionResult> Shorten([FromBody] GenerateShortCodeCommand command,
        CancellationToken cancellationToken = default)
        => OK(await _mediatR.Send(command, cancellationToken));

    [HttpGet, Route("{shortCode}")]
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

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] ListOfLinksQuery listOfLinksQuery,
        CancellationToken cancellationToken = default)
        => Ok(await _mediatR.Send(listOfLinksQuery, cancellationToken));

    [HttpGet]
    public async Task<IActionResult> Count([FromQuery] CountVisitShortenLinkQuery countOfLinksQuery,
        CancellationToken cancellationToken = default)
        => Ok(await _mediatR.Send(countOfLinksQuery, cancellationToken));

    [HttpDelete]
    public async Task<IActionResult> DisableLink([FromRoute] int id,
        CancellationToken cancellationToken = default)
        => OK(await _mediatR.Send(new DisableLinkCommand() { Id = id }, cancellationToken));
}