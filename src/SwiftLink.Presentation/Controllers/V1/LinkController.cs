using MediatR;
using Microsoft.AspNetCore.Mvc;
using SwiftLink.Application.UseCases.Links.Commands;
using SwiftLink.Application.UseCases.Links.Queries;
using SwiftLink.Application.UseCases.Subscribers.Queries;
using SwiftLink.Presentation.Filters;

namespace SwiftLink.Presentation.Controllers.V1;

public class LinkController(ISender sender) : BaseController(sender)
{
    [HttpPost]
    public async Task<IActionResult> Shorten([FromBody] GenerateShortCodeCommand command,
        CancellationToken cancellationToken = default)
        => OK(await MediatR.Send(command, cancellationToken));

    [HttpGet, Route("{shortCode}")]
    [HeaderExtraction]
    public async Task<IActionResult> Visit(string shortCode, [FromQuery] string password,
        CancellationToken cancellationToken = default)
    {
        HttpContext.Items.TryGetValue("ClientMetaData", out var clientMetaData);
        var response = await MediatR.Send(new VisitShortenLinkQuery()
        {
            ShortCode = shortCode,
            Password = password,
            ClientMetaData = clientMetaData.ToString()
        }, cancellationToken);

        return response.IsSuccess ? Redirect(response.Data) : Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] ListOfLinksQuery listOfLinksQuery,
        CancellationToken cancellationToken = default)
        => Ok(await MediatR.Send(listOfLinksQuery , cancellationToken));

    [HttpGet]
    public async Task<IActionResult> Count([FromQuery] CountVisitShortenLinkQuery countOfLinksQuery,
        CancellationToken cancellationToken = default)
        => Ok(await MediatR.Send(countOfLinksQuery, cancellationToken));

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateLinkCommand updateLinkCommand,
        CancellationToken cancellationToken = default)
        => Ok(await MediatR.Send(updateLinkCommand, cancellationToken));

    [HttpDelete]
    public async Task<IActionResult> Disable([FromRoute] int id,
        CancellationToken cancellationToken = default)
        => OK(await MediatR.Send(new DisableLinkCommand(id), cancellationToken));
}
