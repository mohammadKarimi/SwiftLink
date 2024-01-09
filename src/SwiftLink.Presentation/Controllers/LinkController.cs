using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SwiftLink.Application.UseCases.Links.GenerateCommand;

namespace SwiftLink.Presentation.Controllers;

[ApiVersion("1.0")]
public class LinkController : BaseController
{
    [HttpPost]
    public IActionResult Generate([FromBody] GenerateShortCodeCommand command)
    {
        return Ok(MediatR.Send(command));
    }
}
